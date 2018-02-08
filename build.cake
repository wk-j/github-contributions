var name = "GitHubContributions";

var project = $"src/{name}/{name}.fsproj";

Task("Publish")
    .Does(() => {
        DotNetCorePublish(project, new DotNetCorePublishSettings {
            OutputDirectory = "publish/GitHubContributions"
        });
    });

Task("Zip")
    .IsDependentOn("Publish")
    .Does(() => {
        Zip($"publish/GitHubContributions", "publish/github-contributions.0.2.0.zip");
    });


Task("Build").Does(() => {
    MSBuild(project, settings => {
        settings.WithTarget("Build");
    });
});

Task("Run")
    .IsDependentOn("Build")
    .Does(() => {
        MSBuild(project, settings => {
            settings.WithTarget("Run");
        });
});

var target = Argument("target", "default");
RunTarget(target);