using PKHeX.Core;
using System.Collections.Generic;

namespace PkCompletionist.Core;

internal class CompletionValidator3 : CompletionValidatorX
{
    public CompletionValidator3(Command command, SAV3 sav, bool living) : base(command, sav, living)
    {
        this.sav = sav;
        this.unobtainableItems = new List<int>() { 44, 52, 53, 54, 55, 56, 57, 58, 59, 60, 61, 62, 72, 82, 87, 88, 89, 90, 91, 92, 99, 100, 101, 102, 105, 112, 113, 114, 115, 116, 117, 118, 119, 120, 176, 177, 178, 226, 227, 228, 229, 230, 231, 232, 233, 234, 235, 236, 237, 238, 239, 240, 241, 242, 243, 244, 245, 246, 247, 248, 249, 250, 251, 252, 253, 266, 267, 276, 277, 347, 348, 349, 350, 351, 352, 353, 354, 355, 356, 357, 358, 359, 360, 361, 362, 363, 364, 365, 366, 367, 368, 369, 373, 374  };
        // more than that if us-only
    }

    new SAV3 sav;

}