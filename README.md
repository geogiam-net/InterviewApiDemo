# Introduction 
This a small Demo of a web api project to show my skill and knowledge, of course there is still more than one could add in real software, like unit tests, loggers, translators, more services, etc, that is out of the scope and requirements.
Now that I mention the requirements, it was asked to use RabbitMQ, since I didn't knew the library, I needed to program a both sender and client to make sure the functionality works, because of that the Demo.RabbitClient project is an optional plus.

# Getting Started
What you need to run the project:
1.	Install .Net 10
2.	Install Visual Studio 2026
3.	Install draw.io to open the diagrams
4.	Installing SqlServer 2025 is optional 
5.	Oopening an account with RabbitMQ is optional
6.	Creating secrets.json for ConnectionStrings is optional

# Build and Test
Out of the box, one only needs to compile the Demo.Api and it should work, it will use mocks as infrastructure.

To test the web api, one can could use swagger, but I think it is easier to go to Demo.UnitTests -> HttpTests and activate the endpoints with the requests at:

* User-Get.http
* User-Post.http

Some requests are wrong on purpose to test that errors or validations are generated and formed well.

If you want to try to run Demo.Api with real infrastructure, then go to the file Program.cs inside Demo.Api, and switch useMocks to false, then you have to create the connection strings and place them inside secrets.json.

For more information on how to: https://learn.microsoft.com/en-us/aspnet/core/security/app-secrets?view=aspnetcore-10.0&tabs=windows

The two required Ids are ApiDemoDb and ApiDemoRabbit, the end result should look like the following:

{
    "ConnectionStrings": {
	    "ApiDemoDb": "Server=.......",
	    "ApiDemoRabbit": "amqps://......."
    }
}

If the real infrastructure is set, then one can as well execute the Demo.RabbitClient in parallel to watch the messages arrive.