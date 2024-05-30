
# Chapter_0.1.1_Initial_Scope

As a part of this projects' scope, I wanted to share the development notes and the thought process behind this project.

Initial idea was to create a tool to better assess tickers in stock market. That idea was simple:
create a calculator tool that would provide the intrinsic value of a stock. I did not expect much of this, just a simple side project and task to research. It was good enough to pique my curiosity.

But as my stock market (and finance) knowledge grew so did the scope of this project. I kind of had big ideas for software architecture, so I was always nurturing this project to become big in that sense.

Have in mind, that I'm writing this during the timeline where project has taken some kind of a shape and there are clear goals and plans. You can check the [roadmap](/technical/roadmap) for more details.

But, of course, at the very beginning the idea was simple -> Console App that would display Intrinsic Value seemed just fine.

Nevertheless, the requirements were identified in the following manner:
1. Acquire financial data based on the ticker input.
2. Execute ticker valuation with acquired financial data.
3. Display the intrinsic value.

## Chapter_0.1.1.1_Acquire_Financial_Data

Of course, I started researching and analyzing different APIs and offers, but I didn't want to spend any money initially. As I enjoyed web scraping in the past quite a bit, and I never made a comprehensive scraping project with clean architecture, I decided to give it a shot. It was for my own use, so it seemed fine.

Since the language of choice is C#, the library that I decided to use is HtmlAgilityPack.
Important thing about this library in terms of scraping is that it support only static scraping. No JS can be executed with this library.

Here, I started waging heavily on projects potential. Combined with the wish to create a clean and purposeful architecture I started too grandiose. I've read this lesson on many occasions, but still had to try myself. Nevertheless, 

> **Lessons Learned**: Start small and build big afterwards.

I took too long to set up a project. I would first build a version (of what will become FinanceScraper), than scrape it up, and so on. Many refactors and rewrites have gone into this.

There will be a specific segment for FinanceScraper's shape further down.

## Chapter_0.1.1.2_Execute_Ticker_Valuation

This had a totally different challenge than the previous one. I couldn't go too grandiose here as the calculation was straight forward. The challenge here was the domain knowledge. 

I was simply not fluent in stock market and its data. But it was fun to learn new thing(s).

The only valuation formula that was used in the initial scope was [Benjamin Graham formula](/knowledge-hub/benjamin-graham).

Displaying the results was pretty straight forwardly in Console.

To sum up, the initial high level architecture looked like this.
![Chapter_0.1.1_HL_Architecture.png](https://localhost:5001/media/Chapter_0.1.1_HL_Architecture.png)


# Chapter_0.1.2_More_Research_More_Features

In the meantime, I was aggressively researching and investing, so I was obviously finding new, more complex ways to calculate intrinsic value based on some other parameters.

The new formula that triggered many changes in the code was [Discounted Cash Flow formula](/knowledge-hub/discounted-cash-flow).

Essentially, the requirement list now got updated:
1. Acquire data for Benjamin Graham formula.
2. Execute Benjamin Graham ticker valuation.
3. Acquire data for Discounted Cash Flow formula.
4. Execute Discounted Cash Flow valuation.
5. Display acquired data and valuation results.

## Chapter_0.1.2.1_FinanceScraper_Reshape

As new requirements came, it got me thinking heavily on totally refactoring FinanceScraper as it was bound to become a mess. For FinanceScraper, this was the approach I decided to take:
1. Mediatr Pattern with Commands for setting parameters and Handlers for initialization and handling the service.
	1. Now, RequestCommands were covering a specific web page. Only a handful of data was scraped (necessary parameters), but the option to expand was left open. Essentially, the naming convention was {WebSource}{WebPageName}Command.
	2. It's also worth noting that Commands have multiple inheritance, where Constants.Url were set based on the {WebSource} and Constants.UrlPath were set in the constructor, depending on {WebPageName}. Url resolving then was happening in these Commands.
2. IScrapeService with generic argument T, where T : Command - for web scraping execution. Its implementation provides more specific details. Because of the argument, each implementation is defined by the command.

This approach was offering potential versatility and was also pretty straight forward. If extending with new scraping sources, like websites or specific website pages was necessary, the steps were clear.
## Chapter_0.1.2.2_IntrinsicValuation_Reshape

IntrinsicValuation service took on a similar architectural approach as FinanceScraper, in order to fulfil the new requirements.
1. Mediatr Pattern with Commands for setting parameters and Handlers for initialization and handling the service.
2. IValuationService with generic argument T, where T : Command - for valuation execution. Its implementation provides more specific details. Because of the argument, each implementation is defined by the command.

Similarly as for FinanceScraper, versatility and straight forward extension were a key factor for choosing this approach.

More of a challenge here was executing the formula in the right manner.

To conclude this chapter, the UML Activity Diagram of the projects shape as of Chapter 0.1.2.

![Chapter_0.1.2_UML_Activity_Diagram.png](https://localhost:5001/media/Chapter_0.1.2_UML_Activity_Diagram.png)

# Chapter_0.1.3_Going_Online

After using the tool extensively myself to indicate *which tickers are good enough for further research*, the results got me into thinking whether this tool could be published and used by a wider audience.

I went online to research the current state in this use case, and noticed two things immediately:
1. There is a lot of software that does this.
2. None of them are free.

This makes perfect sense. Active investing is "looking at a bunch of financial parameters that are looking like they wanna steal your money" and, where money is - there is money to be made.
Plus maintaining software costs money.

The challenge was to integrate already existing codebase, which is essentially a local web scraper, into an online version.

The updated requirement list:
1. Acquire data for Benjamin Graham formula.
2. Execute Benjamin Graham ticker valuation.
3. Acquire data for Discounted Cash Flow formula.
4. Execute Discounted Cash Flow valuation.
5. **Initiate the process via web interface.**
6. **Display acquired data and valuation results via web interface.**

The tool of choice for displaying content online (aka. Frontend) was Blazor Web Assembly.

> **Blazor WebAssembly (WASM)**
> Blazor WebAssembly (WASM) is a single-page application framework that runs .NET code directly in the browser using WebAssembly. It allows developers to build interactive web applications with C# and .NET instead of JavaScript. Blazor WASM provides a full .NET runtime in the browser, enabling the use of existing .NET libraries and code sharing between server and client.

The reason for going with Blazor WASM was to have a backend-less project. Some thought behind going backend-less:
1. **Offloading backend logic to user's browser** - considering the requirements, there is no reason to have an API for the project.
2. **Deployment cost** - from standard DB-API-Frontend/API-Frontend architecture includes deployment for each of the components. This can get costly. By reducing the number of components we're reducing the maintenance cost.
3. **Learning new tech** - It's kind of fun.

## Chapter_0.1.3.1_Challenges

While attempting to implement existing components into the Blazor.WASM project, right of the bat I've got the following exception:

> System.PlatformNotSupportedException: Operation is not supported on this platform.

Couple of googling and gpt promps afterwards, the info I've got is:

>The `System.PlatformNotSupportedException` error you're encountering in your Blazor WebAssembly application indicates that the code is attempting to execute a functionality that WebAssembly doesn't support.

Analysis results concluded that loading content straight from the web page via HtmlAgilityPack in web assembly app isn't gonna work. 
Instead I had to use System.Net.Http capabilities to acquire HTML content and parse it as a string.

And lastly, CORS.

The web scrape request had to have some CORS headers. Best way to achieve this is to have an API that would act as a proxy and serve the headers for the request.

With above debugging steps resolved, the working version was on! It looked like this on a high level.

![Chapter_0.1.3_Going_Online 2.png](https://localhost:5001/media/Chapter_0.1.3_Going_Online 2.png)

# Chapter_0.1.4_Online_Stabilization

Even though online version of this app was working, it wasn't user friendly at all. Exceptions that were breaking the app occurred more often then not and I wanted for the evaluation to have different variations of calculation that should be executed. 

To get back on the track, the new requirement list for this app looked like this:
1. Acquire data for Benjamin Graham formula.
2. Execute Benjamin Graham ticker valuation.
3. Acquire data for Discounted Cash Flow formula.
4. Execute Discounted Cash Flow valuation.
5. Initiate the process via web interface.
6. Display acquired data and valuation results via web interface.
7. **Enable calculation types to be executed separately.**
8. **Enable users to edit the acquired data prior to evaluation.**
9. **Enable users to edit the acquired data in case of data acquisition failure.**
10. **Handle exceptions with comprehensive set of details.**

As the requirement list has grown again, I figured it would've been good to refactor some more.
In this iteration I've identified two problems worth addressing:
1. I didn't like the fact that Blazor WebAssembly app was directly communicating with components like FinanceScraper and IntrinsicValuation.
2. Considering that separate execution will be introduced there is a certain caution for future issues. For example:
	- Execute only Benjamin Graham formula
	- Execute only DCF formula
	- Execute both Benjamin Graham and DCF formula
	 For each new type of calculation the variation list would grow exponentially, which means a more optimal way is required to handle these.
 3. Standardizing responses. DTO introduction.

## Chapter_0.1.4.1_Introducing_LINK_component

I figured it would be good to have a centralized place for calling current and future logic specific to the initiators logic. Coming through is Financial.Collection.Link component, which goas hand in hand with Financial.Collection.Domain component.

Let me walk you through their purpose:
1. I have a plan to build a portfolio of apps for managing personal finances. Apps like Budgeting app, Savings app, Possession Portfolio app, and so on. As many of these apps might use the existing complonent, a centralized place for *LINKING* logic is appropriate.
2. None of the previously mentioned apps within Financial.Collection should have a direct communication with ServiceComponents. This will keep the project dependencies in check.
3. Initiator project, in this case Blazor WASM for intrinsic value valuation (from now called Intrinsicly), will communicate with LINK components and then LINK components will forward the requests based on the method that Initiator called.

To perhaps paint a better picture of the idea, here is a draft idea of the overall Financial.Collection architecture:
![Chapter_0.1.4_HL_Financial.Collection.Architecture.png](https://localhost:5001/media/Chapter_0.1.4_HL_Financial.Collection.Architecture.png)

Updated Intrinsicly.WASM architecture with LINK components:
![Chapter_0.1.4_Link_Intro.png](https://localhost:5001/media/Chapter_0.1.4_Link_Intro.png)

