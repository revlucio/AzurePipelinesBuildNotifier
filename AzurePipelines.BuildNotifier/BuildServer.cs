using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.TeamFoundation.Build.WebApi;
using Microsoft.VisualStudio.Services.Common;
using Microsoft.VisualStudio.Services.WebApi;

namespace AzurePipelines.BuildNotifier
{
    public class BuildServer
    {
        private readonly BuildHttpClient _buildClient;

        public BuildServer(string collectionUri, string token)
        {
            var connection = new VssConnection(new Uri(collectionUri), new VssBasicCredential("build-api", token));

            _buildClient = connection.GetClient<BuildHttpClient>();
            _buildClient.GetDefinitionsAsync();
        }

        public bool WasLastBuildSuccessful(string projectName, int buildId)
        {
            return _buildClient.GetBuildsAsync(projectName, new[] { buildId })
                       .Result
                       .Where(build => build.Status == BuildStatus.Completed)
                       .OrderBy(build => build.StartTime)
                       .Last()
                       .Result == BuildResult.Succeeded;
        }
    }
}