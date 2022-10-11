# Sample dotnet application
A sample dotnet application built on dotnet C# version 6.0. The docker image is built on Alpine linux 6.0. 

The web application does the following:
* displays the hosting environment details & the current system time
* invokes a remote backend
* uses appsettings for different environments like development, production
* accepts Environment Variables namely
    * ASPNETCORE_ENVIRONMENT (dotnet in-built)
    * DB_NAME (custom)

```bash
# Run the application by loading the appsettings file
dotnet run --environment Production

dotnet run --environment Development
```

![alt txt](/images/dotnetapp1.jpg)

![alt txt](/images/dotnetapp2.jpg)

The docker image can be downloaded from docker hub by running the below command. The docker image is located at https://hub.docker.com/repository/docker/abhinabsarkar/dotnetapp
```cmd
docker pull abhinabsarkar/dotnetapp:<version>
```
Size of the docker image is 115 MB.

## Build the application
```bash
# Build the application using dotnet version 6.0
cd src
dotnet build
# Run the application
dotnet run
```
## Dockerize the application.
```bash
# Build the docker image
docker build -t dotnetapp .
# Run the docker container locally
docker run --name dotnetapp-container -d -p 8002:80 dotnetapp
# Test the app
curl http://localhost:8002
# log into the running container 
docker exec -it dotnetapp-container /bin/bash
docker exec -it <container-name> <command>
# Remove the container
docker rm dotnetapp-container -f 
# Remove the image
docker rmi dotnetapp
```

## Push the image to container registry
```bash
# Push the image to docker hub
docker login
# Tag the local image & map it to the docker repo
docker tag local-image:tagname new-repo:tagname
# eg: docker tag dotnetapp abhinabsarkar/dotnetapp
# push the tagged image to the docker hub
docker push new-repo:tagname
# eg: docker push abhinabsarkar/dotnetapp
```

## Azure pipeline yaml
How to build the [azure-pipelines.yml](azure-pipelines.yml) & host it on Azure Web App?

[Configuring CI/CD Pipelines as Code with YAML in Azure DevOps](https://azuredevopslabs.com/labs/azuredevops/yaml/)

The azure yaml pipeline with different stages (build & deploy), manual validation, approval, etc are shown in the pipeline.

![alt txt](/images/az-pipeline-yaml-cicd.jpg)

### References
Some of the best practices to secure Azure pipeline

1. Add pipeline permissions to a repository resource - [Add protection to a repository resource - Azure Pipelines | Microsoft Docs](https://docs.microsoft.com/en-us/azure/devops/pipelines/process/repository-resource?view=azure-devops#add-pipeline-permissions-to-a-repository-resource)  
    * When you use YAML pipelines, you can easily copy the name of a protected resource (for example, a service connection for your production environment) and include that in a different pipeline. Pipeline permissions protect against such copying. Restricting a repository to specific pipelines prevents an unauthorized pipeline in your project from using your repository. Once enabled, only yaml pipeline defined in the repository will be allowed to use it. All the classic pipelines can use this resource.
2. Define approvals and checks on the repository - [Add protection to a repository resource - Azure Pipelines | Microsoft Docs](https://docs.microsoft.com/en-us/azure/devops/pipelines/process/repository-resource?view=azure-devops#add-a-repository-resource-check)
    * Add Approvals, so a manual approver is required for each time a pipeline requests the repository.
3. Add manual validation before the deployment can be initiated - [Azure pipelines yaml - Control deployments with gates and approvals - Azure Pipelines | Microsoft Docs](https://docs.microsoft.com/en-us/azure/devops/pipelines/release/deploy-using-approvals?view=azure-devops#set-up-manual-validation)
    * Manual validation is especially useful in scenarios where you want to pause a pipeline and validate configuration settings or build packages before starting a computation-intensive job.
    * Sample - [azure-pipelines.yml](azure-pipelines.yml) - Repos --> ManualValidation
4. Define approvals & check for an environment in Azure yaml pipeline - [Create target environment - Azure Pipelines | Microsoft Docs](https://docs.microsoft.com/en-us/azure/devops/pipelines/process/environments?view=azure-devops); [Pipeline deployment approvals - Azure Pipelines | Microsoft Docs](https://docs.microsoft.com/en-us/azure/devops/pipelines/process/approvals?view=azure-devops&tabs=check-pass); [Pre-deployment approvals in Azure DevOps multistage pipelines](https://gavincampbell.dev/post/azure-devops-predeployment-approval-multistage-pipeline/)
    * An environment is a collection of resources that you can target with deployments from a pipeline. Typical examples of environment names are Dev, Test, QA, Staging, and Production. 
    * Sample - azure-pipelines.yml - Repos --> Search for environment 
5. Deployment gates - [Control releases with deployment gates - Azure Pipelines | Microsoft Docs](https://docs.microsoft.com/en-us/azure/devops/pipelines/release/approvals/gates?view=azure-devops)
    * Gates allow automatic collection of health signals from external services and then promote the release when all the signals are successful or stop the deployment on timeout. Typically, gates are used in connection with incident management, problem management, change management, monitoring, and external approval systems.
