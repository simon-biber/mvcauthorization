using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using MvcAuthorization.AuthorizationDescriptors;
using MvcAuthorization.Tests.Fixtures;
using NUnit.Framework;

namespace MvcAuthorization.Tests
{
    [TestFixture]
    public class AuthorizationProviderTests
    {

        [SetUp]
        public void Init()
        {
        }

        #region Test Cases

        [Test]
        [Description("Ensures that a user is denied access if they do not have the required role")]
        public void Should_Fail_If_Not_In_Role()
        {
            IAuthorizationProvider authorizationProvider = new AuthorizationProviderFixture("Admin", null);

            // User is a non-admin
            SetPrincipal(new string[] { "NonAdmin" });

            bool isAuthorized = authorizationProvider.IsAuthorizedAction("Home", "Index");

            // Not authorized
            Assert.IsFalse(isAuthorized);
        }

        [Test]
        [Description("Ensures that a user is granted access if they have the required role")]
        public void Should_Grant_Access_If_In_Role()
        {
            IAuthorizationProvider authorizationProvider = new AuthorizationProviderFixture("Admin", null);

            // User is a non-admin
            SetPrincipal(new string[] { "Admin" });

            bool isAuthorized = authorizationProvider.IsAuthorizedAction("Home", "Index");

            // Not authorized
            Assert.IsTrue(isAuthorized);
        }

        [Test]
        [Description("Ensures that the authorization provider (base) can handle NULL policy descriptor and role fields by returning IsAuthorized = true")]
        public void Handles_Null_Role_And_Policy_Descriptor()
        {
            IAuthorizationProvider authorizationProvider = new AuthorizationProviderFixture(null, null);

            // User is a non-admin
            SetPrincipal(new string[] { "Admin" });

            // Should return true
            bool isAuthorized = false;
            Assert.DoesNotThrow(() => isAuthorized = authorizationProvider.IsAuthorizedAction("Home", "Index"));
            Assert.IsTrue(isAuthorized);
        }

        [Test]
        [Description("Ensures that the authorization provider (base) can handle NULL role/descriptor lists and returns Authorized")]
        public void Handles_Null_Role_Policy_List()
        {
            IAuthorizationProvider authorizationProvider = new AuthorizationProviderFixture();

            // User is a non-admin
            SetPrincipal(new string[] { "Admin" });

            // Should return true
            bool isAuthorized = false;
            Assert.DoesNotThrow(() => isAuthorized = authorizationProvider.IsAuthorizedAction("Home", "Index"));
            Assert.IsTrue(isAuthorized);
        }

        #endregion

        #region Private Helpers

        private void SetPrincipal(string[] roles)
        {
            Thread.CurrentPrincipal = new GenericPrincipal(new GenericIdentity("Test"), roles);
        }

        #endregion

    }
}
