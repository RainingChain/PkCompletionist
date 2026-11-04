import { test } from 'node:test';
import assert from 'node:assert/strict';
import { readFileSync, readdirSync } from 'node:fs';
import { createHash } from 'node:crypto';
import init, * as bindings from '../pkg/pkcompletionist_wasm.js';
import { createApi } from '../main.js';

// Use the web build in Node, supplying bytes in place of the browser's fetch.
await init({ module_or_path: readFileSync(new URL('../pkg/pkcompletionist_wasm_bg.wasm', import.meta.url)) });
const root = new URL('../../tests/PkCompletionist.Tests/', import.meta.url);
const read = path => readFileSync(new URL(path, root));
const fixture = path => JSON.parse(read(path));
const hash = bytes => createHash('sha256').update(bytes).digest('hex').toUpperCase();
const games = ['pinball-ruby-sapphire', 'shuffle', 'ranger', 'red-rescue-team', 'explorers-of-sky', 'crystal', 'yellow', 'emerald'];

for (const id of games) {
    test(`${id}: descriptor through browser API`, () => {
        const expected = fixture(`Fixtures/Descriptors/${id}.json`);
        const api = createApi(bindings);
        try {
            assert.deepEqual(api.SavDescriptor.Execute(read(`Saves/${expected.SaveFile}`), expected.VersionHint), expected.Descriptor);
            assert.deepEqual(api.Command.GetMessages(), expected.Messages);
            assert.equal(api.Meta.GetVersion(), '1.2');
        } finally { api.dispose(); }
    });
    for (const [objective, number] of [['normal', 0], ['living', 1]]) {
        test(`${id}: ${objective} completion through browser API`, () => {
            const expected = fixture(`Fixtures/Completion/${id}.${objective}.json`);
            const api = createApi(bindings);
            try {
                const input = read(`Saves/${expected.SaveFile}`);
                assert.equal(hash(input), expected.SaveSha256);
                assert.equal(api.CompletionValidator.Execute(input, expected.VersionHint, number), expected.Success);
                assert.deepEqual(api.CompletionValidator.GetLastObtainedStatus(), expected.ObtainedStatus);
                assert.deepEqual(api.CompletionValidator.GetLastCompletionHints(), expected.CompletionHints);
                assert.deepEqual(api.Command.GetMessages(), expected.Messages);
            } finally { api.dispose(); }
        });
    }
}

for (const id of readdirSync(new URL('Fixtures/Events/', root))) {
    if (!games.some(game => id.startsWith(`${game}.`))) continue;
    test(`${id}: event through browser API`, () => {
        const expected = fixture(`Fixtures/Events/${id}/result.json`);
        const api = createApi(bindings);
        try {
            assert.equal(api.EventSimulator.Execute(expected.Event, read(`Saves/${expected.SaveFile}`), expected.VersionHint), expected.Success);
            assert.deepEqual(api.Command.GetMessages(), expected.Messages);
            assert.equal(hash(api.Command.GetSavA()), expected.OutputSaveASha256);
            assert.equal(api.Command.GetSavB().length, 0);
        } finally { api.dispose(); }
    });
}

test('Crystal flag analysis through browser API', () => {
    const expected = fixture('Fixtures/crystal.flag-analyzer.json');
    const api = createApi(bindings);
    try {
        assert.equal(api.FlagAnalyzer.Execute(read(`Saves/${expected.SaveFile}`), read(`Saves/${expected.SecondSaveFile}`), expected.ChangeRanges, expected.VersionHint), expected.Success);
        assert.deepEqual(api.Command.GetMessages(), expected.Messages);
        assert.equal(hash(api.Command.GetSavA()), expected.OutputSaveASha256);
        assert.equal(hash(api.Command.GetSavB()), expected.OutputSaveBSha256);
    } finally { api.dispose(); }
});

test('independent API instances retain separate command state', () => {
    const first = createApi(bindings);
    const second = createApi(bindings);
    try {
        assert.equal(first.CompletionValidator.Execute(read('Saves/Pokemon Pinball - Ruby & Sapphire.sav'), '', 0), true);
        assert.equal(second.CompletionValidator.Execute(new Uint8Array(), '', 0), false);
        assert.deepEqual(first.Command.GetMessages(), []);
        assert.ok(second.Command.GetMessages().length > 0);
        assert.equal(first.CompletionValidator.GetLastObtainedStatus()[0], 'pokemon');
        assert.deepEqual(second.CompletionValidator.GetLastObtainedStatus(), []);
    } finally { first.dispose(); second.dispose(); }
});

test('Emerald sorting matches complete C# output hash through browser API', () => {
    const api = createApi(bindings);
    try {
        assert.equal(api.PkSorter.Execute(read('Saves/Pokemon Emerald.sav'), ''), true);
        assert.equal(hash(api.Command.GetSavA()), read('Fixtures/emerald.sorted-save.sha256.txt').toString());
        assert.deepEqual(api.Command.GetMessages(), []);
    } finally { api.dispose(); }
});
