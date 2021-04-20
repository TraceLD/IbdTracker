/*
 * Data comes from the NHSBSA BNF Code Information report dataset (NHSBSA, 2021)
 *
 * [NHSBSA BNF Code Information], NHSBSA Copyright [2021]
 * This information is licenced under the terms of the Open Government Licence.
 * Available at: https://applications.nhsbsa.nhs.uk/infosystems/welcome
 */

using System;

namespace IbdTracker.Core.CommonDtos
{
    public class MedicationDto
    {
        public Guid MedicationId { get; set; }
        public string BnfChapter { get; set; } = null!;
        public int BnfChapterCode { get; set; }
        public string BnfSection { get; set; } = null!;
        public int BnfSectionCode { get; set; }
        public string? BnfParagraph { get; set; }
        public int BnfParagraphCode { get; set; }
        public string? BnfSubparagraph { get; set; }
        public int BnfSubparagraphCode { get; set; }
        public string BnfChemicalSubstance { get; set; } = null!;
        public string BnfChemicalSubstanceCode { get; set; } = null!;
        public string? BnfProduct { get; set; }
        public string BnfProductCode { get; set; } = null!;
        public string BnfPresentation { get; set; } = null!;
        public string BnfPresentationCode { get; set; } = null!;
    }
}