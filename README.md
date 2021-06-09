**KubeAssist** is an Open source web application to perform day to day operations with Kubernetes clusters similar to Kubectl command line tool. It’s an extendable architecture initiated by Braveior Technologies LLP in April 2021. The main goal of KubeAssist is to provide a playground for the Braveior Academy members to research and expand their R&D skills on .NET 5 , Blazor and  Kubernetes enhancing their programming and Innovation skills.
https://www.braveior.com
[![kube-assist](https://raw.githubusercontent.com/braveior/KubeAssist/main/kube-assist-play.png)](https://www.youtube.com/watch?v=IrbJ2Ps56Cs)
![alt text](https://raw.githubusercontent.com/braveior/KubeAssist/main/kube-assist-arch.png "Kube Assist Architecture")  

Braveior also provides a reference Visual Cloud native Foundation Course covering an end – end Modern Product development Experience.  

HighLights:
1. Native Cloud Develpment Architecture
2. Modern Database technologies
3. Micro services 
4. Blazor web development 
5. Devops with CI/CD pipeline 
6. Deployment with Minikube (Kubernetes Environment) 
7. Monitoring with Promethues and Grafana

Course Table of Contents
https://www.braveior.com/courses/cloud-native-computing-foundation-with-net-and-kubernetes/  

**Dont miss the Sample videos** :blush:

KubeAssist – Setup :

Pre-requisites for Development:
1. Visual Studio .NET Community Edition or above
2. Elastic Search – Open source version. (https://www.elastic.co/guide/en/elasticsearch/reference/current/install-elasticsearch.html)
3. Minikube - Desktop Kubernetes Edition for Development purpose  (https://minikube.sigs.k8s.io/docs/start/)

KubeAssist uses three metric and data providers for Kubernetes. 

1. Kube-State-Metrics (https://github.com/kubernetes/kube-state-metrics)
2. metrics-server (https://github.com/kubernetes-sigs/metrics-server)
3. kubernetes api server

kube-state-metrics can be installed using Braveior's customized helm chart or from the above kube-state-metrics 
https://github.com/braveior/helmcharts

metrics-server can be enabled in minikube using the command
**minikube** addons enable metrics-server
https://kubernetes.io/docs/tutorials/hello-minikube/



##Running kube-assist from local laptop running Minikube

Kube-Assist has two components 

1. Kube-assit-agent (.NET 5 Background worker)
2. kube-assist (Blazor Server app)

**Running kube-assist-agent from visual studio for development purpose** - https://github.com/braveior/KubeAssist/tree/main/Braveior.KubeAssist.Agent

It has three dependencies

1. Elastic Search url to be changed in the ExecuteAsync method (worker.cs file)
2. Kubernetes C# Client API method **BuildConfigFromConfigFile** to be used in the ExecuteAsync method (worker.cs file)
3. kube-state-metrics endpoint to be added in the GenerateMetrics method (worker.cs file)
   Once you install kube-state-metrics using the helm chart from braveior , use the below command from minikube to get the URL.
   (Please note the command line window should be running for the URL to be active in minikube environment)

   minikube service --url <<service name for kube-state-metrics>>
   
   which is 

   minikube service --url kube-state-metrics

   for example : http://127.0.0.1:57267

**Running kube-assist blazor server from visual studio for development purpose** - https://github.com/braveior/KubeAssist/tree/main/Braveior.KubeAssist

It has one dependency

1. Elastic Search url to be changed in the KubernetesService class.(KubernetesService.cs file)
  (Class under refactoring currently)

Try the Cloud Native Foundation course to get a good grip on the subject conceptually to begin the Research in this area.

Happy Research :slightly_smiling_face:
