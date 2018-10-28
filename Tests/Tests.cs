using System;
using Microsoft.VisualStudio.Services.Common;
using Microsoft.VisualStudio.Services.WebApi;
using Xunit;

namespace Tests
{
    public class Tests
    {
        [Fact]
        public void Test1()
        {
            var collectionUri = "https://dev.azure.com/revlucio/street-runner";
            var token = "befbvymjfrdx6ba74xnm732s6x2gicvdcfcbanxydnbeo7p3rlra";
            var connection = new VssConnection(new Uri(collectionUri), new VssBasicCredential(string.Empty, token));

//            var witClient = connection.GetClient<WorkItemTrackingHttpClient>();
//            var items = witClient.GetQueriesAsync(teamProjectName).Result;
        }
    }
}