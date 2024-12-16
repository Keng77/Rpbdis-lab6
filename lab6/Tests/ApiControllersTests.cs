using lab6.Controllers;
using lab6.Data;
using lab6.DTO;
using lab6.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Moq;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace Tests
{
    public class ApiControllersTests
    {
        // EnterprisesAPIController Tests
        private readonly EnterprisesAPIController _enterprisesController;
        private readonly Mock<DbSet<Enterprise>> _mockEnterpriseSet;
        private readonly Mock<InspectionsDbContext> _mockEnterpriseContext;

        // InspectionsAPIController Tests
        private readonly InspectionsAPIController _inspectionsController;
        private readonly Mock<DbSet<Inspection>> _mockInspectionSet;
        private readonly Mock<InspectionsDbContext> _mockInspectionContext;

        // InspectorsAPIController Tests
        private readonly InspectorsAPIController _inspectorsController;
        private readonly Mock<DbSet<Inspector>> _mockInspectorSet;
        private readonly Mock<InspectionsDbContext> _mockInspectorContext;

        public ApiControllersTests()
        {
            // EnterprisesAPIController setup
            _mockEnterpriseSet = new Mock<DbSet<Enterprise>>();
            _mockEnterpriseContext = new Mock<InspectionsDbContext>();
            _mockEnterpriseContext.Setup(m => m.Enterprises).Returns(_mockEnterpriseSet.Object);
            _enterprisesController = new EnterprisesAPIController(_mockEnterpriseContext.Object);

            // InspectionsAPIController setup
            var inspectionData = new List<Inspection>
            {
                new Inspection { InspectionId = 1, ProtocolNumber = "PN123" },
                new Inspection { InspectionId = 2, ProtocolNumber = "PN456" }
            }.AsQueryable();

            _mockInspectionSet = new Mock<DbSet<Inspection>>();
            _mockInspectionSet.As<IQueryable<Inspection>>().Setup(m => m.Provider).Returns(inspectionData.Provider);
            _mockInspectionSet.As<IQueryable<Inspection>>().Setup(m => m.Expression).Returns(inspectionData.Expression);
            _mockInspectionSet.As<IQueryable<Inspection>>().Setup(m => m.ElementType).Returns(inspectionData.ElementType);
            _mockInspectionSet.As<IQueryable<Inspection>>().Setup(m => m.GetEnumerator()).Returns(inspectionData.GetEnumerator());

            _mockInspectionContext = new Mock<InspectionsDbContext>();
            _mockInspectionContext.Setup(m => m.Inspections).Returns(_mockInspectionSet.Object);
            _inspectionsController = new InspectionsAPIController(_mockInspectionContext.Object);

            // InspectorsAPIController setup
            _mockInspectorSet = new Mock<DbSet<Inspector>>();
            _mockInspectorContext = new Mock<InspectionsDbContext>();
            _mockInspectorContext.Setup(m => m.Inspectors).Returns(_mockInspectorSet.Object);
            _inspectorsController = new InspectorsAPIController(_mockInspectorContext.Object);
        }

        // EnterprisesAPIController Tests
        [Fact]
        public async Task GetEnterprise_ReturnsNotFound_WhenEnterpriseDoesNotExist()
        {
            var enterpriseId = 999;
            _mockEnterpriseContext.Setup(m => m.Enterprises.FindAsync(enterpriseId)).ReturnsAsync((Enterprise)null);

            var result = await _enterprisesController.GetEnterprise(enterpriseId);

            Assert.IsType<NotFoundResult>(result.Result);
        }

        [Fact]
        public async Task PostEnterprise_CreatesEnterprise()
        {
            var newEnterprise = new Enterprise { Name = "New Enterprise" };
            _mockEnterpriseSet.Setup(m => m.AddAsync(newEnterprise, default)).ReturnsAsync((Microsoft.EntityFrameworkCore.ChangeTracking.EntityEntry<Enterprise>)null);

            var result = await _enterprisesController.PostEnterprise(newEnterprise);

            var createdAtActionResult = Assert.IsType<CreatedAtActionResult>(result.Result);
            Assert.Equal("GetEnterprise", createdAtActionResult.ActionName);
            Assert.Equal(newEnterprise, createdAtActionResult.Value);
        }

        [Fact]
        public async Task DeleteEnterprise_ReturnsNoContent_WhenEnterpriseIsDeleted()
        {
            var enterpriseId = 1;
            var enterprise = new Enterprise { EnterpriseId = enterpriseId };
            _mockEnterpriseContext.Setup(m => m.Enterprises.FindAsync(enterpriseId)).ReturnsAsync(enterprise);

            var result = await _enterprisesController.DeleteEnterprise(enterpriseId);

            Assert.IsType<NoContentResult>(result);
        }

        [Fact]
        public async Task PostInspection_CreatesInspection()
        {
            // Создаем объект DTO вместо Inspection
            var newInspectionDto = new CreateInspectionDto { ProtocolNumber = "PN123" };

            // Настраиваем мок для создания Inspection из DTO
            var newInspection = new Inspection { ProtocolNumber = newInspectionDto.ProtocolNumber };
            _mockInspectionSet.Setup(m => m.AddAsync(It.IsAny<Inspection>(), default)).ReturnsAsync((Microsoft.EntityFrameworkCore.ChangeTracking.EntityEntry<Inspection>)null);

            // Вызываем метод с DTO
            var result = await _inspectionsController.PostInspection(newInspectionDto);

            var createdAtActionResult = Assert.IsType<CreatedAtActionResult>(result.Result);
            Assert.Equal("GetInspection", createdAtActionResult.ActionName);
            Assert.Equal(newInspection.ProtocolNumber, ((Inspection)createdAtActionResult.Value).ProtocolNumber);
        }

        [Fact]
        public async Task DeleteInspection_ReturnsNoContent_WhenInspectionIsDeleted()
        {
            var inspectionId = 1;
            var inspection = new Inspection { InspectionId = inspectionId };
            _mockInspectionContext.Setup(m => m.Inspections.FindAsync(inspectionId)).ReturnsAsync(inspection);

            var result = await _inspectionsController.DeleteInspection(inspectionId);

            Assert.IsType<NoContentResult>(result);
        }

        // InspectorsAPIController Tests
        [Fact]
        public async Task GetInspector_ReturnsNotFound_WhenInspectorDoesNotExist()
        {
            var inspectorId = 999;
            _mockInspectorContext.Setup(m => m.Inspectors.FindAsync(inspectorId)).ReturnsAsync((Inspector)null);

            var result = await _inspectorsController.GetInspector(inspectorId);

            Assert.IsType<NotFoundResult>(result.Result);
        }

        [Fact]
        public async Task PostInspector_CreatesInspector()
        {
            var newInspector = new Inspector { FullName = "Inspector Name" };
            _mockInspectorSet.Setup(m => m.AddAsync(newInspector, default)).ReturnsAsync((Microsoft.EntityFrameworkCore.ChangeTracking.EntityEntry<Inspector>)null);

            var result = await _inspectorsController.PostInspector(newInspector);

            var createdAtActionResult = Assert.IsType<CreatedAtActionResult>(result.Result);
            Assert.Equal("GetInspector", createdAtActionResult.ActionName);
            Assert.Equal(newInspector, createdAtActionResult.Value);
        }

        [Fact]
        public async Task DeleteInspector_ReturnsNoContent_WhenInspectorIsDeleted()
        {
            var inspectorId = 1;
            var inspector = new Inspector { InspectorId = inspectorId };
            _mockInspectorContext.Setup(m => m.Inspectors.FindAsync(inspectorId)).ReturnsAsync(inspector);

            var result = await _inspectorsController.DeleteInspector(inspectorId);

            Assert.IsType<NoContentResult>(result);
        }
    }
}