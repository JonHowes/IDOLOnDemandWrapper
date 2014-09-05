using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

using IODWrapper;

namespace Demonstrator
{

	class Program
	{

		static void Main(string[] args)
		{

			string IODApiKey = "PUT YOUR IDOL OnDemand API key here";

			List<string> negativeTopics = new List<string>();
			List<string> positiveTopics = new List<string>();
			List<string> people = new List<string>();
			List<string> companies = new List<string>();

			try
			{
				string testFileFolder = @".\TestFiles";

				IODJobBatchRequest req = new IODJobBatchRequest();
				req.ApiKey = IODApiKey;

				string filePath1 = System.IO.Path.Combine(testFileFolder, "TheFile01.txt");
				req.AddFile("file01", filePath1);

				string filePath2 = System.IO.Path.Combine(testFileFolder, "TheFile02.txt");
				req.AddFile("file02", filePath2);

				SentimentRequest sr1 = new SentimentRequest(IODSource.text, "The excellent Lebron James jumped over the lazy Kobe Bryant");
				req.AddRequest(sr1);
				sr1.OnResponse = (status, response) =>
				{
					RecordSentiment(status, response, negativeTopics, positiveTopics);
				};

				SentimentRequest sr2 = new SentimentRequest(IODSource.file, "file01");
				req.AddRequest(sr2);
				sr2.OnResponse = (status, response) =>
				{
					RecordSentiment(status, response, negativeTopics, positiveTopics);
				};

				EntityRequest er1 = new EntityRequest(IODSource.text, "The excellent Lebron James jumped over the lazy Kobe Bryant", "people_eng");
				req.AddRequest(er1);
				er1.OnResponse = (status, response) =>
				{
					RecordPeople(status, response, people);
				};

				EntityRequest er2 = new EntityRequest(IODSource.file, "file01", "people_eng");
				req.AddRequest(er2);
				er2.OnResponse = (status, response) =>
				{
					RecordPeople(status, response, people);
				};

				EntityRequest er3 = new EntityRequest(IODSource.file, "file02", "companies_eng");
				req.AddRequest(er3);
				er3.OnResponse = (status, response) =>
				{
					RecordCompanies(status, response, companies);
				};

				EntityRequest er4 = new EntityRequest(IODSource.url, "http://www.bbc.co.uk/news/business/companies/", "companies_eng");
				req.AddRequest(er4);
				er4.OnResponse = (status, response) =>
				{
					RecordCompanies(status, response, companies);
				};

				Console.WriteLine("Making request ...");
				req.MakeRequest();

				Console.WriteLine("\r\nResponse:");
				ListToScreen("Negative sentiment", negativeTopics);
				ListToScreen("Positive sentiment", positiveTopics);
				ListToScreen("People", people);
				ListToScreen("Companies", companies);

			}
			catch (Exception e)
			{
				Console.WriteLine(string.Format("\r\nException: {0}", e.Message));
			}

			Console.WriteLine("\r\nPress key to exit");
			Console.ReadKey();
		}


		static private void RecordSentiment(IODStatus status, SentimentResponse response, List<string> negativeTopics, List<string> positiveTopics)
		{
			if (status == IODStatus.finished)
			{
				foreach (Sentiment n in response.negative)
				{
					negativeTopics.Add(n.normalized_text);
				}
				foreach (Sentiment p in response.positive)
				{
					positiveTopics.Add(p.normalized_text);
				}
			}
			else
			{
				Console.WriteLine("Sentiment request: {0}", status);
			}
		}

		static private void RecordPeople(IODStatus status, EntityResponse response, List<string> people)
		{
			if (status == IODStatus.finished)
			{
				foreach (Entity e in response.entities)
				{
					people.Add(e.normalized_text);
				}
			}
			else
			{
				Console.WriteLine("People entity request: {0}", status);
			}
		}

		static private void RecordCompanies(IODStatus status, EntityResponse response, List<string> companies)
		{
			if (status == IODStatus.finished)
			{
				foreach (Entity e in response.entities)
				{
					companies.Add(e.normalized_text);
				}
			}
			else
			{
				Console.WriteLine("Company entity request: {0}", status);
			}
		}

		static private void ListToScreen(string heading, List<string> stuff)
		{
			Console.Write("\r\n{0}:  ", heading);
			foreach (string s in stuff)
			{
				Console.Write("{0}, ", s);
			}
			Console.WriteLine("");
		}

	}

}
