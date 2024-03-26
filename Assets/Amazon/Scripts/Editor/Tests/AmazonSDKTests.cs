using System;
using NUnit.Framework;
using UnityEngine;

// ReSharper disable Unity.IncorrectMonoBehaviourInstantiation

namespace Tests
{

    public class AmazonSDKTests : AmazonTest
    {
        [Test]
        public void EmitAdLoadedEventShouldTriggerOnAdLoadedEvent()
        {
            TestEmitAdLoadedEvent(new AmazonAds.Android.AndroidAdResponse());
        }


        private static void TestEmitAdLoadedEvent(AmazonAds.AdResponse response)
        {
            const string successMessage = "OnAdLoadedEvent triggered.";
            AmazonAds.Amazon.OnSuccessDelegate successHandler = (_response) => {
                Assert.That(_response, Is.EqualTo(response));
                Debug.Log(successMessage);
            };

            const string failureMessage = "OnAdFailedEvent triggered.";
            AmazonAds.Amazon.OnFailureDelegate failureHandler = (_error) => {
                Debug.Log(failureMessage);
            };


            try {
                successHandler.Invoke(response);
                failureHandler.Invoke("123");
            } finally {

            }
        }
    }
}
