using IbdTracker.Core.CommonDtos;
using IbdTracker.Core.Entities;

namespace IbdTracker.Core.Extensions
{
    public static class MedicationExtensions
    {
        public static MedicationDto ToDto(this Medication m) =>
            new()
            {
                MedicationId = m.MedicationId,
                BnfChapter = m.BnfChapter,
                BnfChapterCode = m.BnfChapterCode,
                BnfSection = m.BnfSection,
                BnfSectionCode = m.BnfSectionCode,
                BnfParagraph = m.BnfParagraph,
                BnfParagraphCode = m.BnfParagraphCode,
                BnfSubparagraph = m.BnfSubparagraph,
                BnfSubparagraphCode = m.BnfSubparagraphCode,
                BnfChemicalSubstance = m.BnfChemicalSubstance,
                BnfChemicalSubstanceCode = m.BnfChemicalSubstanceCode,
                BnfProduct = m.BnfProduct,
                BnfProductCode = m.BnfProductCode,
                BnfPresentation = m.BnfPresentation,
                BnfPresentationCode = m.BnfPresentationCode
            };
    }
}