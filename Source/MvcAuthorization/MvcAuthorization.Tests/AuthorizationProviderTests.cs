using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using MvcAuthorization.AuthorizationDescriptors;
using MvcAuthorization.Policy;
using MvcAuthorization.Tests.Fixtures;
using NUnit.Framework;

namespace MvcAuthorization.Tests
{
    [TestFixture]
    public class AuthorizationProviderTests
    {
        private PolicyFixture _policyFixture = new PolicyFixture();

        [SetUp]
        public void Init()
        {
            AuthorizationProvider.ResolveDependenciesUsing(t =>
                        {
                            if (_policyFixture != null && t == typeof(PolicyFixture))
                            {
                                return _policyFixture;
                            }
                            return null;
                        });
        }

        #region Test Cases

        #region Role Tests

        [Test]
        [Description("Ensures that a user is denied access if they do not have the required role")]
        public void Should_Deny_Access_If_Not_In_Role()
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

        #endregion

        #region Policy Tests

        [Test]
        [Description("Ensures that a user is granted access if the policy allows it")]
        public void Should_Grant_Access_If_Policy_Satisfied()
        {
            IAuthorizationProvider authorizationProvider = new AuthorizationProviderFixture(null, new PolicyAuthorizationDescriptor(false, "TestPolicyFixture"));
            _policyFixture.IsAuthorizedResult = true;

            bool isAuthorized = authorizationProvider.IsAuthorizedAction("Home", "Index");

            // Authorized
            Assert.IsTrue(isAuthorized);
        }

        [Test]
        [Description("Ensures that a user is denied access if the policy does not allow it")]
        public void Should_Deny_Access_If_Policy_Not_Satisfied()
        {
            IAuthorizationProvider authorizationProvider = new AuthorizationProviderFixture(null, new PolicyAuthorizationDescriptor(false, "TestPolicyFixture"));
            _policyFixture.IsAuthorizedResult = false;

            bool isAuthorized = authorizationProvider.IsAuthorizedAction("Home", "Index");

            // Not authorized
            Assert.IsFalse(isAuthorized);
        }

        #endregion

        #region Role and policy tests

        [Test]
        [Description("Ensures that a user is granted access only if the policy and role allow it")]
        public void Should_Grant_Access_Only_If_Role_And_Policy_Satisfied()
        {
            IAuthorizationProvider authorizationProvider = new AuthorizationProviderFixture("Admin", new PolicyAuthorizationDescriptor(false, "TestPolicyFixture"));
            bool isAuthorized;

            // 1) Policy allowed but role not...expect not allowed
            _policyFixture.IsAuthorizedResult = true;
            SetPrincipal(new string[] { "AnonymousUser" });
            isAuthorized = authorizationProvider.IsAuthorizedAction("Home", "Index");
            Assert.IsFalse(isAuthorized);

            // 2) Policy not but and role is...expect not allowed
            _policyFixture.IsAuthorizedResult = false;
            SetPrincipal(new string[] { "Admin" });
            isAuthorized = authorizationProvider.IsAuthorizedAction("Home", "Index");
            Assert.IsFalse(isAuthorized);

            // 3) Policy and role not allowed...expect not allowed
            _policyFixture.IsAuthorizedResult = false;
            SetPrincipal(new string[] { "AnonymousUser" });
            isAuthorized = authorizationProvider.IsAuthorizedAction("Home", "Index");
            Assert.IsFalse(isAuthorized);

            // 2) Policy and rolw allowed...expect allowed
            _policyFixture.IsAuthorizedResult = true;
            SetPrincipal(new string[] { "Admin" });
            isAuthorized = authorizationProvider.IsAuthorizedAction("Home", "Index");
            Assert.IsTrue(isAuthorized);
        }

        #endregion

        #region Null Tests

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
        public void Handles_Null_Role_And_Policy_List()
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

        #endregion

        #region Private Helpers

        private void SetPrincipal(string[] roles)
        {
            Thread.CurrentPrincipal = new GenericPrincipal(new GenericIdentity("Test"), roles);
        }

        #endregion

    }
}
