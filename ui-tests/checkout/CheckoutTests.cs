
using Microsoft.Extensions.Configuration;
using Microsoft.Playwright;
using Microsoft.Playwright.NUnit;
using NUnit.Framework;

namespace PlaywrightTests;

[Parallelizable(ParallelScope.Self)]
[TestFixture]
public class CheckoutTests : PageTest
{
    private static IPage page;

    [SetUp]
    public async Task SetUp()
    {
        var configuration = new ConfigurationBuilder()
       .AddJsonFile("appsettings.json")
       .Build();

        var BASE_URL = configuration["Xyz:BASE_URL"];

        var browser = await Playwright.Chromium.LaunchAsync(new BrowserTypeLaunchOptions
        {
            Headless = false
        });
        page = await browser.NewPageAsync();
        await page.GotoAsync(BASE_URL);
        await page.GetByLabel("Prihvatam").ClickAsync();
        await page.EvaluateAsync("window.scrollBy(0, 1000)");
        await page.Locator("#newsletterModal").GetByLabel("Close").ClickAsync();
    }



    [Test]
    public async Task CheckIfTheItemAddedToTheCart()
    {
        await page.GetByLabel("PANTALONE").ClickAsync();
        await page.GetByRole(AriaRole.Button, new() { Name = " Dodaj u korpu" }).ClickAsync();
        await page.GetByRole(AriaRole.Link, new() { Name = "Pogledaj korpu" }).ClickAsync();

        await Expect(page.GetByRole(AriaRole.Link, new() { Name = "PANTALONE", Exact = true })).ToBeVisibleAsync();

        var cartCountText = await page.Locator(".mini-cart .count.cartCount").Locator("visible=true").TextContentAsync();

        Assert.That(cartCountText, Is.EqualTo("1"));

    }

    //THIS IS A BUG THAT I DISCOVERED DURING WRITTING OF THESE TESTS AND IT WILL BE DESCRIBED IN THE DOCUMENT
    //DIALOGS ARE DISMISED BY PLAYWRIGHT BUT STATE IS STILL NOT UPDATED
    //https://playwright.dev/dotnet/docs/dialogs
    [Test]
    public async Task CheckIsTheNumberOfItemsUpdatedInTheCart()
    {
        await page.GetByLabel("PANTALONE").ClickAsync();
        await page.GetByRole(AriaRole.Button, new() { Name = " Dodaj u korpu" }).ClickAsync();
        await page.GetByRole(AriaRole.Link, new() { Name = "Pogledaj korpu" }).ClickAsync();
        await page.Locator(".btn-number.qtyplus").ClickAsync();

        var cartCountText = await page.Locator(".mini-cart .count.cartCount").Locator("visible=true").TextContentAsync();

        Assert.That(cartCountText, Is.EqualTo("2"));
    }

    [Test]
    public async Task CheckIsTheWrongCouponForDiscountAccepted()
    {
        await page.GetByLabel("ELEGANTNA OBUĆA").ClickAsync();
        await page.GetByRole(AriaRole.Button, new() { Name = " Dodaj u korpu" }).ClickAsync();
        await page.GetByLabel("Slažem se sa Opštim uslovima.").CheckAsync();
        await page.GetByRole(AriaRole.Button, new() { Name = "IDI NA PLAĆANJE" }).ClickAsync();
        await page.GetByPlaceholder("Kod za popust").ClickAsync();
        await page.GetByPlaceholder("Kod za popust").FillAsync("greska");
        await page.GetByLabel("Apply Discount Code").ClickAsync();
        await page.GetByLabel("Kod za popust").FillAsync("greska");
        await page.GetByLabel("Apply Discount Code").ClickAsync();
        await page.GetByRole(AriaRole.Complementary).Locator("div").Filter(new() { HasText = "Šoping korpaSlika" }).Nth(1).ClickAsync();

        await Expect(page.GetByText("Enter a valid discount code")).ToBeVisibleAsync();
    }

    [Test]
    public async Task CheckTheRequiredFieldsForContactAndDeliveryInfo()
    {
        await page.GetByLabel("PANTALONE").ClickAsync();
        await page.GetByRole(AriaRole.Button, new() { Name = " Dodaj u korpu" }).ClickAsync();
        await page.GetByLabel("Slažem se sa Opštim uslovima.").CheckAsync();
        await page.GetByRole(AriaRole.Button, new() { Name = "IDI NA PLAĆANJE" }).ClickAsync();
        await page.GetByPlaceholder("Email").FillAsync("fake@yopmail.com");
        await page.GetByLabel("Pick up in store").CheckAsync();
        await page.GetByPlaceholder("Ime", new() { Exact = true }).ClickAsync();
        await page.GetByPlaceholder("Ime", new() { Exact = true }).FillAsync("Nikola");
        await page.GetByPlaceholder("Prezime").FillAsync("Test");
        await page.GetByPlaceholder("Kompanija (neobavezno)").FillAsync("Nul Tien");
        await page.GetByPlaceholder("Adresa").FillAsync("Placeholder 95");
        await page.GetByPlaceholder("Apartman, suite, etc").FillAsync("41");
        await page.GetByLabel("Poštanski broj").FillAsync("11070");
        await page.GetByPlaceholder("Grad").FillAsync("Novi Sad");
        await page.GetByLabel("Telefon (opcija)").FillAsync("063 4567089");

        //I would check if the order can be completed, but since this is not a test environment, I will not do that
    }

    /*
    Here are some bug/test case examples: 

     * BUG: 

    Update cart quantity is not working
    Description:
    When you try to change the quantity of an item which is selected in the cart, you get an error.
    Steps to reproduce:
     - open the website "https://rs.shop.xyz.fashion"
     - add an item to the cart
     - open the cart
     - click on the "+" button for the "Kolicina"
     - observe the error message
    Expected results: The quantity is changed by 1 each time you press the "+" button, and the "Total" amount is changed accordingly.
    Actual results: The message "error update cart" is displayed.

    Operating System: Win 11
    Browser: Firefox 127.0

     * TEST CASE:

    Test Case 1: Verify Home Page Loading
    Test Case ID: TC001
    Test Description: Verify that the home page loads successfully.
    Preconditions: None
    Test Steps:
     - Navigate to https://rs.shop.xyz.fashion/.
    Expected Result: The home page should load completely without errors.

    Test Case 2: Verify Search Functionality
    Test Case ID: TC002
    Test Description: Verify the search functionality with a valid product name.
    Preconditions: User is on the home page.
    Test Steps:
     - Enter a valid product name (e.g., "dress") in the search bar.
     - Click the search button.
    Expected Result: The search results page should display products related to the search term.

    Test Case 3: Verify Search with Invalid Item
    Test Case ID: TC003
    Test Description: Verify the search functionality with an invalid product name.
    Preconditions: User is on the home page.
    Test Steps:
     - Enter an invalid product name (e.g., "xyzabc") in the search bar.
     - Click the search button.
    Expected Result: The search results page should display a message indicating no products were found.

    Test Case 4: Verify Product Detail Page
    Test Case ID: TC004
    Test Description: Verify that a product detail page loads successfully.
    Preconditions: User is on the search results page.
    Test Steps:
     - Click on a product from the search results.
    Expected Result: The product detail page should load, displaying detailed information about the selected product.

    Test Case 5: Verify Add to Cart Functionality
    Test Case ID: TC005
    Test Description: Verify that a product can be added to the cart.
    Preconditions: User is on a product detail page.
    Test Steps:
     - Click the "Add to Cart" button.
    Expected Result: The product should be added to the cart, and a cart preview on the right side should be displayed.

    Test Case 6: Verify View Cart Functionality
    Test Case ID: TC006
    Test Description: Verify that the user can view the cart.
    Preconditions: User has added at least one product to the cart.
    Test Steps:
     - Click the "Cart" icon/link.
    Expected Result: The cart page should display, showing all the products added to the cart.
    */
}