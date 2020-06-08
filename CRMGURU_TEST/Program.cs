using System;
using System.Linq;
using Microsoft.Extensions.Configuration;
using Microsoft.EntityFrameworkCore;
using System.IO;
using System.Text;
using System.Net.Http;
using System.Text.Json;
using System.Collections.Generic;

namespace CRMGURU_TEST
{
	class Program
	{
		static DbContextOptions dco;
		static void Main(string[] args)
		{
			var builder = new ConfigurationBuilder();
			builder.SetBasePath(Environment.CurrentDirectory);
			builder.AddJsonFile("appsettings.json");
			var config = builder.Build();
			string cs = config.GetConnectionString("DefaultConnection");

			dco = new DbContextOptionsBuilder<CountriesDbContext>()
				.UseSqlServer(cs)
				.Options;
			var rk = '\0';
			while (rk != '3')
			{
				rk = ConsoleChat.MenuForUser("Управление\n\t1 - перейти к поиску информации по стране\n\t2 - Вывести все известные страны\n\t3 - Выйти");
				var rez = "";
				try
				{
					rez = rk switch
					{
						'1' => FindCountry(),
						'2' => GetAll(),
						'3' => "\nПока\n",
						_ => "\nОтвет не правильный\n"
					};
				}
				catch (Exception ex)
				{
					rez = "Что-то пошло не по плану\nПроверьте настройки и введённую строку";
				}
				ConsoleChat.WriteLine(rez);

			}
		}

		static string FindCountry()
		{
			
			var result = "";
			var str = ConsoleChat.ChatWithUser("Введите название страны").ToLower();
			using var db = new CountriesDbContext(dco);
			db.LoadAll();
			var country = db.Countries.Where(x => x.Name.ToLower() == str || x.CountryCode.ToLower() == str || x.Capital.Name.ToLower() == str).FirstOrDefault();
			if (country == null)
			{
				using var client = new HttpClient();
				str = str.Replace(' ', '+');

				var response = client.GetAsync($"https://restcountries.eu/rest/v2/name/{str}").Result;
				if (!response.IsSuccessStatusCode)
					response = client.GetAsync($"https://restcountries.eu/rest/v2/name/{str}?fullText=true").Result;
				if (response.IsSuccessStatusCode)
				{
					country = JsonSerializer.Deserialize<List<Country>>(response.Content.ReadAsStringAsync().Result).FirstOrDefault();
					ConsoleChat.WriteLine(Country.GetHeader(), '\n',country?.ToString());
					var rk = ConsoleChat.MenuForUser("Нажмите '1' ещё раз, чтобы сохранить результат в БД");
					if (rk == '1')
					{
						if (SaveCountry(country, db))
							result = "Готово";
						else
							result = "Уже есть в базе";
					}
					else
						result = "Как хотите";
				}
				else
					result = "Запрос не удачен";
			}
			else
				result = Country.GetHeader() + '\n' + country.ToString();
			return result;
		}

		static bool SaveCountry(Country country, CountriesDbContext db)
		{
			var dbcity = db.Cities.Where(x => x.Name == country.Capital.Name).FirstOrDefault();
			if (dbcity != null)
				country.Capital = dbcity;
			var dbregion = db.Regions.Where(x => x.Name == country.Region.Name).FirstOrDefault();
			if (dbregion != null)
				country.Region = dbregion;
			var dbCountry = db.Countries.Where(x => x.CountryCode == country.CountryCode).FirstOrDefault();
			if (dbCountry != null)
				return false;
			if (dbCountry == null)
				db.Countries.Add(country);
			db.SaveChanges();
			return true;
		}

		static string GetAll()
		{
			var strBuilder = new StringBuilder();
			using var db = new CountriesDbContext(dco);
			db.LoadAll();
			strBuilder.Append(Country.GetHeader());
			foreach (var country in db.Countries)
				strBuilder.Append(country.ToString()).Append('\n');
			return strBuilder.ToString();
		}

		



	}
}
