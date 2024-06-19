// using System.Text.RegularExpressions;
// using System.Threading.Tasks;
// using Microsoft.Playwright;
// using Microsoft.Playwright.NUnit;
// using NUnit.Framework;

// namespace PlaywrightTests;

// [Parallelizable(ParallelScope.Self)]
// [TestFixture]
// public class ExampleTest : PageTest
// {
//     [Test]
//     public async Task HappyFlow()
//     {
//         await Page.GotoAsync("https://rs.shop.xyz.fashion/");

//         var desiredItem = await Page.QuerySelectorAsync("[data-p-id='8514566848751']");
//         await desiredItem.ClickAsync();
//         await Page.ClickAsync("#AddToCart-template--16822699917551__main");
//         await Page.CheckAsync("#agree_checkbox");
//         await Page.ClickAsync("#checkout_button");


//         // Example action: Get text content from the element
//         // if (desiredItem != null)
//         // {
//         //     var textContent = await desiredItem.TextContentAsync();
//         //     Console.WriteLine("Element text: " + textContent);
//         // }
//         // else
//         // {
//         //     Console.WriteLine("Element not found.");
//         // }
//     }

// }