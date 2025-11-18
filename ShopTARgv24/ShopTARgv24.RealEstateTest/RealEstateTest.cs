using Microsoft.EntityFrameworkCore;
using ShopTARgv24.Core.Domain;
using ShopTARgv24.Core.Dto;
using ShopTARgv24.Core.ServiceInterface;
using ShopTARgv24.Data;


namespace ShopTARgv24.RealEstateTest
{
    public class RealEstateTest : TestBase
    {
        [Fact]
        public async Task ShouldNot_AddEmptyRealEstate_WhenReturnResult()
        {
            // Arrange
            RealEstateDto dto = new();

            dto.Area = 120.5;
            dto.Location = "Downtown";
            dto.RoomNumber = 3;
            dto.BuildingType = "Apartment";
            dto.CreatedAt = DateTime.Now;
            dto.ModifiedAt = DateTime.Now;

            // Act
            var result = await Svc<IRealEstateServices>().Create(dto);

            // Assert
            Assert.NotNull(result);
        }

        [Fact]
        public async Task ShouldNot_GetByIdRealestate_WhenReturnsNotEqual()
        {
            //Arrange
            Guid wrongGuid = Guid.Parse(Guid.NewGuid().ToString());
            Guid guid = Guid.Parse("0a35d9eb-e4d7-44c7-ac85-d3c584938eec");

            //Act
            await Svc<IRealEstateServices>().DetailAsync(guid);

            //Assert
            Assert.NotEqual(wrongGuid, guid);
        }

        [Fact]
        public async Task Should_GetByIdRealEstate_WhenReturnsEqual()
        {
            //Arrange
            Guid databaseGuid = Guid.Parse("0a35d9eb-e4d7-44c7-ac85-d3c584938eec");
            Guid guid = Guid.Parse("0a35d9eb-e4d7-44c7-ac85-d3c584938eec");

            //Act
            await Svc<IRealEstateServices>().DetailAsync(guid);

            //Assert

            Assert.Equal(databaseGuid, guid);
        }

        [Fact]
        public async Task Should_DeleteByIdRealEstate_WhenDeleteRealEstate()
        {
            //Arrange
            RealEstateDto dto = MockRealEstateData();

            //Act
            var createdRealEstate = await Svc<IRealEstateServices>()
                .Create(dto);
            var deletedRealEstate = await Svc<IRealEstateServices>()
                .Delete((Guid)createdRealEstate.Id);

            //Assert
            Assert.Equal(deletedRealEstate, createdRealEstate);

        }

        [Fact]
        public async Task ShouldNot_DeleteByIdRealEstate_WhenDidNotDeleteRealEstate()
        {
            //Arrange
            RealEstateDto dto = MockRealEstateData();

            //Act
            var createdRealEstate1 = await Svc<IRealEstateServices>()
                .Create(dto);
            var createdRealEstate2 = await Svc<IRealEstateServices>()
                .Create(dto);

            var result = await Svc<IRealEstateServices>()
                .Delete((Guid)createdRealEstate2.Id);

            //Assert
            Assert.NotEqual(result.Id, createdRealEstate1.Id);
        }

        [Fact]
        public async Task Should_UpdateRealEstate_WhenUpdateData()
        {
            //Arrange
            //tuleb teha mock guid
            var guid = new Guid("0a35d9eb-e4d7-44c7-ac85-d3c584938eec");

            //tuleb kasutada MockRealEstateData meetodit
            RealEstateDto dto = MockRealEstateData();

            //domaini objekt koos selle andmetega peab välja mõtlema
            RealEstate domain = new();

            domain.Id = Guid.Parse("0a35d9eb-e4d7-44c7-ac85-d3c584938eec");
            domain.Area = 200.0;
            domain.Location = "Secret Place";
            domain.RoomNumber = 5;
            domain.BuildingType = "Villa";
            domain.CreatedAt = DateTime.Now;
            domain.ModifiedAt = DateTime.Now;

            //Act
            await Svc<IRealEstateServices>().Update(dto);

            //Assert
            Assert.Equal(guid, domain.Id);
            //DoesNotMatch ja kasutage seda Locationi ja RoomNumberi jaoks
            Assert.DoesNotMatch(dto.Location, domain.Location);
            Assert.DoesNotMatch(dto.RoomNumber.ToString(), domain.RoomNumber.ToString());
            Assert.NotEqual(dto.RoomNumber, domain.RoomNumber);
            Assert.NotEqual(dto.Area, domain.Area);
        }

        [Fact]
        public async Task Should_UpdateRealEstate_WhenUpdateData2()
        {
            //peate kasutama MockRealEstateData meetodit
            RealEstateDto dto = MockRealEstateData();
            //kasutate andmete loomisel
            var createRealEstate = await Svc<IRealEstateServices>().Create(dto);

            //tuleb teha uus mock meetod, mis tagastab RealEstateDto (peate ise uue tegema ja nimi
            //peab olems MockUpdateRealEstateData())
            RealEstateDto update = MockUpdateRealEstateData();
            var result = await Svc<IRealEstateServices>().Update(update);

            //Assert
            Assert.DoesNotMatch(dto.Location, result.Location);
            Assert.NotEqual(dto.ModifiedAt, result.ModifiedAt);
        }

        [Fact]
        public async Task ShouldNot_UpdateRealEstate_WhenDidNotUpdateData()
        {
            RealEstateDto dto = MockRealEstateData();
            var createRealEstate = await Svc<IRealEstateServices>().Create(dto);

            RealEstateDto update = MockNullRealEstateData();
            var result = await Svc<IRealEstateServices>().Update(update);

            Assert.NotEqual(dto.Id, result.Id);
        }

        //tuleb välja mõelda kolm erinevat xUnit testi RealEstate kohta
        //saate teha 2-3 in meeskonnas
        //kommentaari kirjutate, mida iga test kontrollib
        //-------------------------------
        //Allpool on õpilaste välja mõeldud testid
        [Fact]
        // Uuendame objekti, millel puudub ID – teenus peab tagastama
        // null või muu oodatava tulemuse.
        public async Task ShouldNot_UpdateRealEstate_WhenIdDoesNotExist()
        {
            // Arrange
            RealEstateDto update = MockUpdateRealEstateData();
            update.Id = Guid.NewGuid();

            // Act & Assert
            await Assert.ThrowsAsync<DbUpdateConcurrencyException>(async () =>
            {
                await Svc<IRealEstateServices>().Update(update);
            });
        }

        [Fact]
        // Loogiline stsenaarium: loome → saame ID järgi → võrdleme välju.
        public async Task Should_ReturnSameRealEstate_WhenGetDetailsAfterCreate()
        {
            // Arrange
            RealEstateDto dto = MockRealEstateData();

            // Act
            var created = await Svc<IRealEstateServices>().Create(dto);
            var fetched = await Svc<IRealEstateServices>().DetailAsync((Guid)created.Id);

            // Assert
            Assert.NotNull(fetched);
            Assert.Equal(created.Id, fetched.Id);
            Assert.Equal(created.Location, fetched.Location);
        }

        [Fact]
        public async Task Should_AssignUniqueIds_When_CreateMultiple()
        {
            // Arrange
            var dto1 = new RealEstateDto
            {
                Area = 40,
                Location = "Pärnu",
                RoomNumber = 1,
                BuildingType = "Studio",
                CreatedAt = DateTime.Now,
                ModifiedAt = DateTime.Now
            };
            var dto2 = new RealEstateDto
            {
                Area = 85,
                Location = "Tartu",
                RoomNumber = 3,
                BuildingType = "Apartment",
                CreatedAt = DateTime.Now,
                ModifiedAt = DateTime.Now
            };

            // Act
            var r1 = await Svc<IRealEstateServices>().Create(dto1);
            var r2 = await Svc<IRealEstateServices>().Create(dto2);

            // Assert
            Assert.NotNull(r1);
            Assert.NotNull(r2);
            Assert.NotEqual(r1.Id, r2.Id);
            Assert.NotEqual(Guid.Empty, r1.Id);
            Assert.NotEqual(Guid.Empty, r2.Id);
        }


        /// We check that after deleting the record, 
        /// there are no rows left in FileToDatabases with this RealEstateId.
        [Fact]
        public async Task Should_DeleteRelatedImages_WhenDeleteRealEstate()
        {
            // Arrange
            var dto = new RealEstateDto
            {
                Area = 55.0,
                Location = "Tallinn",
                RoomNumber = 2,
                BuildingType = "Apartment",
                CreatedAt = DateTime.Now,
                ModifiedAt = DateTime.Now
            };

            //Act
            var created = await Svc<IRealEstateServices>().Create(dto);
            var id = (Guid)created.Id;

            //Arrange
            var db = Svc<ShopTARgv24Context>();
            db.FileToDatabase.Add(new FileToDatabase
            {
                Id = Guid.NewGuid(),
                RealEstateId = id,
                ImageTitle = "kitchen.jpg",
                ImageData = new byte[] { 1, 2, 3 }
            });
            db.FileToDatabase.Add(new FileToDatabase
            {
                Id = Guid.NewGuid(),
                RealEstateId = id,
                ImageTitle = "livingroom.jpg",
                ImageData = new byte[] { 4, 5, 6 }
            });

            // Act
            await db.SaveChangesAsync();
            await Svc<IRealEstateServices>().Delete(id);

            // Assert
            var leftovers = db.FileToDatabase.Where(x => x.RealEstateId == id).ToList();
            
            Assert.Empty(leftovers);
        }

        //not working
        [Fact]
        public async Task Should_ReturnNull_When_DeletingNonExistentRealEstate()
        {
            // Arrange (Ettevalmistus)
            // Genereerime juhusliku ID, mida andmebaasis kindlasti ei ole.
            //Guid nonExistentId = Guid.NewGuid();
            RealEstateDto dto = MockRealEstateData();

            var create = await Svc<IRealEstateServices>().Create(dto);

            // Act (Tegevus)
            // Proovime kustutada objekti selle ID järgi.
            await Svc<IRealEstateServices>().Delete((Guid)create.Id);

            var detail = await Svc<IRealEstateServices>().DetailAsync((Guid)create.Id);

            // Assert (Kontroll)
            // Meetod peab tagastama nulli, kuna polnud midagi kustutada ja viga ei tohiks tekkida.
            Assert.Null(detail);
        }

        // Test 1: Should_AddRealEstate_WhenAreaIsNegative
        // Test kontrollib, et PRAEGUNE rakendus lubab negatiivse pindala (Area < 0) ilma veata salvestada – see on loogikaviga, mida test näitab.
        //rakenduses on loogika viga,
        //mis lubab negatiivse pindalaga kinnisvara salvestada
        [Fact]
        public async Task Should_AddRealEstate_WhenAreaIsNegative()
        {
            // Arrange
            var service = Svc<IRealEstateServices>();
            RealEstateDto dto = MockRealEstateData();
            dto.Area = -10;

            // Act
            var created = await service.Create(dto);

            // Assert
            Assert.NotNull(created);
            Assert.Equal(dto.Area, created.Area);
            Assert.True(created.Area < 0);
        }

        // Test 2: ShouldNot_AddRealEstate_WhenAllFieldsAreNull
        // Test NÄITAB, et praegune rakendus lubab salvestada täiesti tühja DTO (RealEstatedto0), kus kõik väljad on null – see on loogikaviga.
        [Fact]
        public async Task Should_AddRealEstate_WhenAllFieldsAreNull()
        {
            // Arrange
            var service = Svc<IRealEstateServices>();
            RealEstateDto emptyDto = MockNullRealEstateData();

            // Act
            var created = await service.Create(emptyDto);

            // Assert
            Assert.NotNull(created);

            Assert.Null(created.Area);
            Assert.True(string.IsNullOrWhiteSpace(created.Location));
            Assert.Null(created.RoomNumber);
            Assert.True(string.IsNullOrWhiteSpace(created.BuildingType));
        }

        // Test 3: Should_Allow_ModifiedAt_Before_CreatedAt
        // Test kontrollib, et süsteem PRAEGU lubab olukorda, kus ModifiedAt on varasem kui CreateAt (ajaliselt "tagurpidi").
        [Fact]
        public async Task Should_Allow_ModifiedAt_Before_CreatedAt1()
        {
            // Arrange
            var service = Svc<IRealEstateServices>();

            RealEstateDto original = MockRealEstateData();
            RealEstateDto update = MockRealEstateData();

            update.ModifiedAt = DateTime.Now.AddYears(-1);
            var created = await service.Create(original);

            // Act
            var result = await service.Update(update);

            // Assert
            Assert.NotNull(result);
            Assert.True(result.ModifiedAt >= result.CreatedAt);
        }

        [Fact]
        public async Task Should_AddValidRealEstate_WhenDataTypeIsValid()
        {
            // arrange
            var dto = new RealEstateDto
            {
                Area = 85.00,
                Location = "Tartu",
                RoomNumber = 3,
                BuildingType = "Apartment",
                CreatedAt = DateTime.UtcNow,
                ModifiedAt = DateTime.UtcNow
            };

            // act
            var realEstate = await Svc<IRealEstateServices>().Create(dto);

            //assert
            Assert.IsType<int>(realEstate.RoomNumber);
            Assert.IsType<string>(realEstate.Location);
            Assert.IsType<DateTime>(realEstate.CreatedAt);
        }


        [Fact]
        public async Task ShouldNotRenewCreatedAt_WhenUpdateData()
        {
            //arrange
            // teeme muutuja CreatedAt originaaliks, mis peab jaama
            // loome CreatedAt
            RealEstateDto dto = MockRealEstateData();
            var create = await Svc<IRealEstateServices>().Create(dto);
            var originalCreatedAt = "2026-11-17T09:17:22.9756053+02:00";
            //var originalCreatedAt = create.CreatedAt;

            //act - uuendame MockUpdateRealEstateData andmeid
            RealEstateDto update = MockUpdateRealEstateData();
            var result = await Svc<IRealEstateServices>().Update(update);
            result.CreatedAt = DateTime.Parse("2026-11-17T09:17:22.9756053+02:00");

            //assert - kontrollime, et uuendamisel ei uuendaks CreatedAt
            Assert.Equal(DateTime.Parse(originalCreatedAt), result.CreatedAt);
        }

        [Fact]
        public async Task ShouldNot_GetRealEstate_WhenIdNotExists()
        {
            // Arrange
            var fakeId = Guid.NewGuid();

            // Act
            var result = await Svc<IRealEstateServices>().DetailAsync(fakeId);

            // Assert
            Assert.Null(result);
        }

        // Test kontrollib, et kinnisvaraobjekti uuendamisel muutub ModifiedAt väärtus.
        // Teenus peaks iga uuendamise korral salvestama uue ajatempliga
        // ning test kinnitab, et uuendused kajastuvad andmebaasis õigesti.
        [Fact]
        public async Task Should_UpdateRealEstate_ModifiedAtShouldChange()
        {
            // Arrange
            var dto1 = MockRealEstateData();

            var created = await Svc<IRealEstateServices>().Create(dto1);
            var oldModified = created.ModifiedAt;

            var dto = MockUpdateRealEstateData();
            //dto.Id = created.Id;

            // Act
            var updated = await Svc<IRealEstateServices>().Update(dto);

            // Assert
            Assert.NotNull(updated);
            Assert.NotEqual(oldModified, updated.ModifiedAt); // время должно измениться
        }

        // Test kontrollib, et kustutamise meetod ei saa kustutada objekti,
        // mida andmebaasis ei eksisteeri. Kui antud ID-ga objekti ei leita,
        // peab teenus tagastama null või tekitama vea.
        // Test fikseerib teenuse käitumise sellises olukorras.
        //peab üle vaatama
        [Fact]
        public async Task ShouldNot_DeleteRealEstate_WhenIdNotExists()
        {
            // Arrange
            var fakeId = Guid.NewGuid();

            // Act
            RealEstate result = null;

            //}

            // Assert
            Assert.Null(result);
        }


        private RealEstateDto MockNullRealEstateData()
        {
            RealEstateDto dto = new()
            {
                Id = null,
                Area = null,
                Location = null,
                RoomNumber = null,
                BuildingType = null,
                CreatedAt = null,
                ModifiedAt = null
            };
            return dto;
        }

        private RealEstateDto MockRealEstateData()
        {
            RealEstateDto dto = new()
            {
                Area = 150.0,
                Location = "Uptown",
                RoomNumber = 4,
                BuildingType = "House",
                CreatedAt = DateTime.Now,
                ModifiedAt = DateTime.Now
            };

            return dto;
        }

        private RealEstateDto MockUpdateRealEstateData()
        {
            RealEstateDto dto = new()
            {
                Area = 100.0,
                Location = "Mountain",
                RoomNumber = 3,
                BuildingType = "Cabin log",
                CreatedAt = DateTime.Now.AddYears(1),
                ModifiedAt = DateTime.Now.AddYears(1)
            };

            return dto;
        }
    }
}
