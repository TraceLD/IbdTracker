/*
 * Data comes from the NHSBSA BNF Code Information report dataset (NHSBSA, 2021)
 *
 * [NHSBSA BNF Code Information], NHSBSA Copyright [2021]
 * This information is licenced under the terms of the Open Government Licence.
 * Available at: https://applications.nhsbsa.nhs.uk/infosystems/welcome
 */

using CsvHelper.Configuration.Attributes;

namespace IbdTracker.Tools.PharmaceuticalImporter
{
    /// <summary>
    /// Represents a BNF Code Information CSV row mapped to a C# POCO.
    /// </summary>
    public class BnfDrug
    {
        // index represents the column number in CSV, when counted from left to right;
        
        [Index(0)]
        public string BnfChapter { get; set; }
        
        [Index(1)]
        public int BnfChapterCode { get; set; }
        
        [Index(2)]
        public string BnfSection { get; set; }
        
        [Index(3)]
        public int BnfSectionCode { get; set; }
        
        [Index(4)]
        public string BnfParagraph { get; set; }
        
        [Index(5)]
        public int BnfParagraphCode { get; set; }
        
        [Index(6)]
        public string BnfSubparagraph { get; set; }
        
        [Index(7)]
        public int BnfSubparagraphCode { get; set; }
        
        [Index(8)]
        public string BnfChemicalSubstance { get; set; }
        
        [Index(9)]
        public string BnfChemicalSubstanceCode { get; set; }
        
        [Index(10)]
        public string BnfProduct { get; set; }
        
        [Index(11)]
        public string BnfProductCode { get; set; }
        
        [Index(12)]
        public string BnfPresentation { get; set; }
        
        [Index(13)]
        public string BnfPresentationCode { get; set; }
    }
}