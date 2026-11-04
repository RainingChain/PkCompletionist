// Browser adapter for the C# JSExport names used by pokemoncompletion.
// Build with `wasm-pack build rust --target web` before calling load().
export function createApi(bindings) {
    const command = new bindings.Command();
    let lastValidation = { ObtainedStatus: [], CompletionHints: [] };
    const result = () => JSON.parse(command.result_json());
    return {
        Command: {
            GetMessages: () => result().Messages,
            GetSavA: () => command.save_a(),
            GetSavB: () => command.save_b(),
        },
        CompletionValidator: {
            Execute(bytes, hint, objective) {
                const success = command.validate(bytes, hint, objective);
                if (success) lastValidation = result();
                return success;
            },
            GetLastObtainedStatus: () => [...lastValidation.ObtainedStatus],
            GetLastCompletionHints: () => [...lastValidation.CompletionHints],
        },
        EventSimulator: {
            Execute: (name, bytes, hint, second = null) => command.event(name, bytes, hint, second),
        },
        FlagAnalyzer: {
            Execute: (before, after, ranges, hint) => command.analyze_flags(before, after, ranges, hint),
        },
        dispose: () => command.free(),
    };
}

export async function load(moduleUrl = new URL('./pkg/pkcompletionist_wasm.js', import.meta.url)) {
    const bindings = await import(moduleUrl.href ?? moduleUrl);
    await bindings.default();
    return createApi(bindings);
}
