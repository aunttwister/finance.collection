
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

Here, I started waging heavily on projects potential. Combined with the wish to create a clean and purposeful architecture I started too grandiose. I've read this lesson on many occasions, but still had to try myself. Nevertheless, 

	Lessons Learned: Start small and build big afterwards.

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
