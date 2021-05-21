using System.Threading.Tasks;
using FluentAssertions;
using IbdTracker.Features.Medications;
using Xunit;

namespace IbdTracker.Tests.Features.Medications
{
    public class GetTests : TestBase
    {
        public GetTests(SharedFixture sharedFixture) : base(sharedFixture)
        {
        }

        [Fact]
        public async Task ShouldGetAllMedicationsWhenNoParameters()
        {
            // arrange;
            var query = new Get.Query(null, null);
            
            // act;
            var res = await SendMediatorRequestInScope(query);
            
            // assert;
            res.Should().NotBeNullOrEmpty();
        }
        
        [Fact]
        public async Task ShouldGetMedicationsWithAGivenSubstanceNameWhenItExists()
        {
            // arrange;
            const string chemicalSubstance = "Budesonide";
            var query = new Get.Query(chemicalSubstance, null);
            
            // act;
            var res = await SendMediatorRequestInScope(query);
            
            // assert;
            res.Should()
                .NotBeEmpty().And
                .OnlyContain(m => m.BnfChemicalSubstance.Equals(chemicalSubstance));
        }

        [Fact]
        public async Task ShouldReturnEmptyListIfTheChemicalSubstanceDoesNotExist()
        {
            // arrange;
            const string chemicalSubstance = "LITERALLY_MADE_UP";
            var query = new Get.Query(chemicalSubstance, null);
            
            // act;
            var res = await SendMediatorRequestInScope(query);
            
            // assert;
            res.Should().BeEmpty();
        }
        
        [Fact]
        public async Task ShouldGetMedicationsWithAGivenProductNameWhenItExists()
        {
            // arrange;
            const string productName = "Cortiment";
            var query = new Get.Query(null, productName);
            
            // act;
            var res = await SendMediatorRequestInScope(query);
            
            // assert;
            res.Should()
                .NotBeEmpty().And
                .OnlyContain(m => m.BnfProduct!.Equals(productName));
        }
        
        [Fact]
        public async Task ShouldReturnEmptyListIfTheProductNameDoesNotExist()
        {
            // arrange;
            const string productName = "LITERALLY_MADE_UP";
            var query = new Get.Query(null, productName);
            
            // act;
            var res = await SendMediatorRequestInScope(query);
            
            // assert;
            res.Should().BeEmpty();
        }
        
        [Fact]
        public async Task ShouldGetMedicationsWhenBothValuesValid()
        {
            // arrange;
            const string chemicalSubstance = "Budesonide";
            const string productName = "Cortiment";
            var query = new Get.Query(chemicalSubstance, productName);
            
            // act;
            var res = await SendMediatorRequestInScope(query);
            
            // assert;
            res.Should()
                .NotBeEmpty().And
                .OnlyContain(m =>
                    m.BnfProduct!.Equals(productName) && m.BnfChemicalSubstance.Equals(chemicalSubstance));
        }
    }
}