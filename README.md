# Airy-ApplicationInsights-Owin #

Use this library when you need Application Insights request and error tracking for **both** ASP.Net MVC and Web API using OWIN running on **IIS**.

If you are self hosting Web API using OWIN then the [applicationinsights-owinextensions](https://github.com/marcinbudny/applicationinsights-owinextensions) project may be a better fit as it works for both IIS and self hosted configurations.

#### Features: ####
  - Correct Operation Name resolution for attribute based routing in both ASP.NET MVC & WebApi (with and without parameter names.)
  - Tracking of errors in the ASP.Net MVC, Web API and OWIN pipelines. Including correlation with tracked requests.

