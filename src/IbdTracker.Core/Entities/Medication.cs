/*
 * Data comes from the NHSBSA BNF Code Information report dataset (NHSBSA, 2021)
 *
 * [NHSBSA BNF Code Information], NHSBSA Copyright [2021]
 * This information is licenced under the terms of the Open Government Licence.
 * Available at: https://applications.nhsbsa.nhs.uk/infosystems/welcome
 */

using System;
using System.Collections.Generic;

namespace IbdTracker.Core.Entities
{
    public class Medication
    {
        public Medication(
            string bnfChapter,
            int bnfChapterCode,
            string bnfSection,
            int bnfSectionCode,
            int bnfParagraphCode,
            int bnfSubparagraphCode,
            string bnfChemicalSubstance,
            string bnfChemicalSubstanceCode,
            string bnfProductCode,
            string bnfPresentation,
            string bnfPresentationCode,
            string? bnfParagraph = null,
            string? bnfSubparagraph = null,
            string? bnfProduct = null
        )
        {
            BnfChapter = bnfChapter;
            BnfChapterCode = bnfChapterCode;
            BnfSection = bnfSection;
            BnfSectionCode = bnfSectionCode;
            BnfParagraph = bnfParagraph;
            BnfParagraphCode = bnfParagraphCode;
            BnfSubparagraph = bnfSubparagraph;
            BnfSubparagraphCode = bnfSubparagraphCode;
            BnfChemicalSubstance = bnfChemicalSubstance;
            BnfChemicalSubstanceCode = bnfChemicalSubstanceCode;
            BnfProduct = bnfProduct;
            BnfProductCode = bnfProductCode;
            BnfPresentation = bnfPresentation;
            BnfPresentationCode = bnfPresentationCode;
        }

        public Guid MedicationId { get; set; }
        public string BnfChapter { get; set; }
        public int BnfChapterCode { get; set; }
        public string BnfSection { get; set; }
        public int BnfSectionCode { get; set; }
        public string? BnfParagraph { get; set; }
        public int BnfParagraphCode { get; set; }
        public string? BnfSubparagraph { get; set; }
        public int BnfSubparagraphCode { get; set; }
        public string BnfChemicalSubstance { get; set; }
        public string BnfChemicalSubstanceCode { get; set; }
        public string? BnfProduct { get; set; }
        public string BnfProductCode { get; set; }
        public string BnfPresentation { get; set; }
        public string BnfPresentationCode { get; set; }

        public List<Prescription> Prescriptions { get; } = new List<Prescription>();
    }
}