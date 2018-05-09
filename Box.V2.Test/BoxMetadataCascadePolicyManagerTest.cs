﻿using Box.V2.Config;
using Box.V2.Managers;
using Box.V2.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Threading.Tasks;

namespace Box.V2.Test
{
    [TestClass]
    public class BoxMetadataCascadePolicyManagerTest : BoxResourceManagerTest
    {
        private readonly BoxMetadataCascadePolicyManager _cascadePolicyManager;

        public BoxMetadataCascadePolicyManagerTest()
        {
            _cascadePolicyManager = new BoxMetadataCascadePolicyManager(Config.Object, Service, Converter, AuthRepository);
        }

        [TestMethod]
        [TestCategory("CI-UNIT-TEST")]
        public async Task CreateMetadataCascadePolicy_ValidResponse()
        {
            /*** Arrange ***/
            string responseString = "{ \"id\": \"84113349-794d-445c-b93c-d8481b223434\", \"type\": \"metadata_cascade_policy\", \"owner_enterprise\": { \"type\": \"enterprise\", \"id\": \"11111\" }, \"parent\": { \"type\": \"folder\", \"id\": \"22222\" }, \"scope\": \"enterprise_11111\", \"templateKey\": \"testTemplate\" }";
            Handler.Setup(h => h.ExecuteAsync<BoxMetadataCascadePolicy>(It.IsAny<IBoxRequest>()))
                .Returns(Task.FromResult<IBoxResponse<BoxMetadataCascadePolicy>>(new BoxResponse<BoxMetadataCascadePolicy>()
                {
                    Status = ResponseStatus.Success,
                    ContentString = responseString
                }));

            /*** Act ***/
            BoxMetadataCascadePolicy cascadePolicy = await _cascadePolicyManager.CreateCascadePolicyAsync("22222", "enterprise_11111", "templateKey");

            /*** Assert ***/
            Assert.AreEqual("84113349-794d-445c-b93c-d8481b223434", cascadePolicy.Id);
            Assert.AreEqual("22222", cascadePolicy.Parent.Id);
            Assert.AreEqual("enterprise_11111", cascadePolicy.Scope);
            Assert.AreEqual("testTemplate", cascadePolicy.TemplateKey);
        }

        [TestMethod]
        [TestCategory("CI-UNIT-TEST")]
        public async Task GetMetadataCascadePolicy_ValidResponse()
        {
            /*** Arrange ***/
            string responseString = "{ \"id\": \"84113349-794d-445c-b93c-d8481b223434\", \"type\": \"metadata_cascade_policy\", \"owner_enterprise\": { \"type\": \"enterprise\", \"id\": \"11111\" }, \"parent\": { \"type\": \"folder\", \"id\": \"22222\" }, \"scope\": \"enterprise_11111\", \"templateKey\": \"testTemplate\" }";
            Handler.Setup(h => h.ExecuteAsync<BoxMetadataCascadePolicy>(It.IsAny<IBoxRequest>()))
                .Returns(Task.FromResult<IBoxResponse<BoxMetadataCascadePolicy>>(new BoxResponse<BoxMetadataCascadePolicy>()
                {
                    Status = ResponseStatus.Success,
                    ContentString = responseString
                }));

            /*** Act ***/
            BoxMetadataCascadePolicy cascadePolicy = await _cascadePolicyManager.GetCascadePolicyAsync("84113349-794d-445c-b93c-d8481b223434");

            /*** Assert ***/
            Assert.AreEqual("84113349-794d-445c-b93c-d8481b223434", cascadePolicy.Id);
            Assert.AreEqual("22222", cascadePolicy.Parent.Id);
            Assert.AreEqual("enterprise_11111", cascadePolicy.OwnerEnterprise.Id);
            Assert.AreEqual("testTemplate", cascadePolicy.TemplateKey);
        }

        [TestMethod]
        [TestCategory("CI-UNIT-Test")]
        public async Task DeleteMetadataCascadePolicy_ValidResponse()
        {
            string responseString = "";
            IBoxRequest boxRequest = null;
            Uri metadataCascadePolicyUri = new Uri(Constants.MetadataCascadePolicyEndpointString);
            Config.SetupGet(x => x.MetadataCascadePolicyUri).Returns(metadataCascadePolicyUri);
            Handler.Setup(h => h.ExecuteAsync<BoxMetadataCascadePolicy>(It.IsAny<IBoxRequest>()))
                .Returns(Task.FromResult<IBoxResponse<BoxMetadataCascadePolicy>>(new BoxResponse<BoxMetadataCascadePolicy>()
                {
                    Status = ResponseStatus.Success,
                    ContentString = responseString
                }))
                .Callback<IBoxRequest>(r => boxRequest = r);

            /*** Act ***/
            bool result = await _cascadePolicyManager.DeleteCascadePolicyAsync("12345");

            /*** Assert ***/
            //Request check
            Assert.IsNotNull(boxRequest);
            Assert.AreEqual(RequestMethod.Delete, boxRequest.Method);
            Assert.AreEqual(metadataCascadePolicyUri + "12345", boxRequest.AbsoluteUri.AbsoluteUri);

            //Response check
            Assert.AreEqual(true, result);
        }

        [TestMethod]
        [TestCategory("CI-UNIT-Test")]
        public async Task ForceApplyMetadataCascadePolicy_ValidResponse()
        {
            string responseString = "";
            IBoxRequest boxRequest = null;
            Uri metadataCascadePolicyUri = new Uri(Constants.MetadataCascadePolicyEndpointString);
            Config.SetupGet(x => x.MetadataCascadePolicyUri).Returns(metadataCascadePolicyUri);
            Handler.Setup(h => h.ExecuteAsync<BoxMetadataCascadePolicy>(It.IsAny<IBoxRequest>()))
                .Returns(Task.FromResult<IBoxResponse<BoxMetadataCascadePolicy>>(new BoxResponse<BoxMetadataCascadePolicy>()
                {
                    Status = ResponseStatus.Success,
                    ContentString = responseString
                }))
                .Callback<IBoxRequest>(r => boxRequest = r);

            /*** Act ***/
            bool result = await _cascadePolicyManager.ForceApplyCascadePolicyAsync("12345", "none");

            /*** Assert ***/
            //Request check
            Assert.IsNotNull(boxRequest);
            Assert.AreEqual(RequestMethod.Post, boxRequest.Method);
            Assert.AreEqual(metadataCascadePolicyUri + "12345", boxRequest.AbsoluteUri.AbsoluteUri);

            //Response check
            Assert.AreEqual(true, result);
        }
    }
}