using IntroSE.Kanban.Backend.ServiceLayer;
using Newtonsoft.Json;

namespace BackendUnitTests
{
    public class Tests
    {
        private ServiceFactory _factory;
        private const string _testUser1 = "testUser1@gmail.com";
        private const string _testUser2 = "testUser2@gmail.com";
        private const string _testMember = "testMember@gmail.com";
        private const string _testPassword = "testPassword1";
        private const string _testBoardName = "testBoardName";

        private Response Deserialize(string json) =>
            JsonConvert.DeserializeObject<Response>(json);

        [OneTimeSetUp]
        public void Setup()
        {
            _factory = new();
            _factory.GetBoardService().DeleteData();
            _factory.GetUserService().Register(_testUser1, _testPassword);
            _factory.GetUserService().Register(_testUser2, _testPassword);
            _factory.GetUserService().Register(_testMember, _testPassword);
        }

        [Test, Order(0)]
        public void CreateBoard_ValidName()
        {
            var res = Deserialize(_factory.GetBoardService().CreateBoard(_testUser1, _testBoardName));
            Assert.That(res.ErrorMessage, Is.Null);
        }

        [Test, Order(1)]
        public void CreateBoard_EmptyName()
        {
            var res = Deserialize(_factory.GetBoardService().CreateBoard(_testUser1, ""));
            Assert.That(res.ErrorMessage, Is.Not.Null);
            StringAssert.Contains("Board name", res.ErrorMessage);
        }

        [Test, Order(2)]
        public void CreateBoard_DuplicateName()
        {
            var res = Deserialize(_factory.GetBoardService().CreateBoard(_testUser1, _testBoardName));
            Assert.That(res.ErrorMessage, Is.Not.Null);
            StringAssert.Contains("already", res.ErrorMessage.ToLower());
        }

        [Test, Order(3)]
        public void CreateBoard_DuplicateNameDifferentUsers()
        {
            var res = Deserialize(_factory.GetBoardService().CreateBoard(_testUser2, _testBoardName));
            Assert.That(res.ErrorMessage, Is.Null);
        }

        [Test, Order(4)]
        public void LimitColumn_ValidLimit()
        {
            var res = Deserialize(_factory.GetBoardService().LimitColumn(_testUser2, _testBoardName, 0, 5));
            Assert.That(res.ErrorMessage, Is.Null);
        }

        [Test, Order(5)]
        public void LimitColumn_NoLimit()
        {
            var res = Deserialize(_factory.GetBoardService().LimitColumn(_testUser2, _testBoardName, 0, -1));
            Assert.That(res.ErrorMessage, Is.Null);
        }

        [Test, Order(6)]
        public void JoinBoard_Valid()
        {
            var res = Deserialize(_factory.GetBoardService().JoinBoard(_testMember, 1));
            Assert.That(res.ErrorMessage, Is.Null);
        }

        [Test, Order(7)]
        public void LimitColumn_ZeroLimit()
        {
            var res = Deserialize(_factory.GetBoardService().LimitColumn(_testUser2, _testBoardName, 0, 0));
            Assert.That(res.ErrorMessage, Is.Not.Null);
            StringAssert.Contains("zero", res.ErrorMessage);
        }

        [Test, Order(8)]
        public void LimitColumn_NegativeLimit()
        {
            var res = Deserialize(_factory.GetBoardService().LimitColumn(_testUser2, _testBoardName, 0, -5));
            Assert.That(res.ErrorMessage, Is.Not.Null);
            StringAssert.Contains("cannot be negative", res.ErrorMessage);
        }

        [Test, Order(9)]
        public void AdvanceTask_NotAssigned()
        {
            Deserialize(_factory.GetTaskService().AddTask(_testUser1, _testBoardName, "Title", "Desc", DateTime.Today.AddDays(1)));
            var res = Deserialize(_factory.GetTaskService().AdvanceTask(_testUser1, _testBoardName, 0, 1));
            Assert.That(res.ErrorMessage, Is.Not.Null);
            StringAssert.Contains("not assigned", res.ErrorMessage.ToLower());
        }

        [Test, Order(10)]
        public void AssignTask_NonMember()
        {
            var res = Deserialize(_factory.GetTaskService().AssignTask(_testUser1, _testBoardName, 0, 1, _testUser2));
            Assert.That(res.ErrorMessage, Is.Not.Null);
            StringAssert.Contains("not a member", res.ErrorMessage.ToLower());
        }

        [Test, Order(11)]
        public void JoinBoard_AlreadyMember()
        {
            var res = Deserialize(_factory.GetBoardService().JoinBoard(_testMember, 1));
            Assert.That(res.ErrorMessage, Is.Not.Null);
            StringAssert.Contains("already", res.ErrorMessage.ToLower());
        }

        [Test, Order(12)]
        public void TransferOwnership_ToNonMember()
        {
            var res = Deserialize(_factory.GetBoardService().TransferOwnership(_testUser1, _testUser2, _testBoardName));
            Assert.That(res.ErrorMessage, Is.Not.Null);
            StringAssert.Contains("not a member", res.ErrorMessage.ToLower());
        }

        [Test, Order(13)]
        public void LeaveBoard_AsOwner()
        {
            var res = Deserialize(_factory.GetBoardService().LeaveBoard(_testUser1, 1));
            Assert.That(res.ErrorMessage, Is.Not.Null);
            StringAssert.Contains("transfer ownership", res.ErrorMessage.ToLower());
        }

        [Test, Order(14)]
        public void DeleteBoard_AsNonOwner()
        {
            var res = Deserialize(_factory.GetBoardService().DeleteBoard(_testMember, _testBoardName));
            Assert.That(res.ErrorMessage, Is.Not.Null);
            StringAssert.Contains("is not the owner of the board", res.ErrorMessage.ToLower());
        }

        [Test, Order(15)]
        public void GetColumnTasks_InvalidIndex()
        {
            var res = Deserialize(_factory.GetBoardService().GetColumnTasks(_testUser1, _testBoardName, 5));
            Assert.That(res.ErrorMessage, Is.Not.Null);
            StringAssert.Contains("Column index", res.ErrorMessage);
        }

        [Test, Order(16)]
        public void AssignTask_NullEmail()
        {
            var res = Deserialize(_factory.GetTaskService().AssignTask(_testUser1, _testBoardName, 0, 1, null));
            Assert.That(res.ErrorMessage, Is.Not.Null);
            StringAssert.Contains("cannot be null", res.ErrorMessage.ToLower());
        }

        [Test, Order(17)]
        public void JoinBoard_NullEmail()
        {
            var res = Deserialize(_factory.GetBoardService().JoinBoard(null, 1));
            Assert.That(res.ErrorMessage, Is.Not.Null);
            StringAssert.Contains("cannot be null", res.ErrorMessage.ToLower());
        }

        [Test, Order(18)]
        public void DeleteBoard_ValidOwner()
        {
            var res = Deserialize(_factory.GetBoardService().DeleteBoard(_testUser2, _testBoardName));
            Assert.That(res.ErrorMessage, Is.Null);
        }

        [Test, Order(19)]
        public void LeaveBoard_ValidMember()
        {
            var res = Deserialize(_factory.GetBoardService().LeaveBoard(_testMember, 1));
            Assert.That(res.ErrorMessage, Is.Null);
        }
    }
}