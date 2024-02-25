// using System.Globalization;
// using System.Text.Json;
// using Microsoft.Playwright;
// using Microsoft.Playwright.NUnit;
// using Microsoft.Playwright.TestAdapter;
// using NUnit.Framework.Interfaces;
// using System.Collections.Concurrent;

// namespace Syki.Tests.E2E;

// public class E2ETestBaseParallelVersion : SykiWorkerAwareTest
// {
//     private Task<IPlaywright> _playwrightTask = null!;

//     public string BrowserName { get; internal set; }

//     public IPlaywright Playwright { get; private set; }

//     public IBrowserType BrowserType { get; private set; }

//     public async Task PlaywrightSetup()
//     {
//         Playwright = await _playwrightTask.ConfigureAwait(continueOnCapturedContext: false);
//         BrowserName = PlaywrightSettingsProvider.BrowserName;
//         BrowserType = Playwright[BrowserName];
//         Playwright.Selectors.SetTestIdAttribute("data-testid");
//     }

//     public ILocatorAssertions Expect(ILocator locator)
//     {
//         return Assertions.Expect(locator);
//     }

//     public IPageAssertions Expect(IPage page)
//     {
//         return Assertions.Expect(page);
//     }

//     public IAPIResponseAssertions Expect(IAPIResponse response)
//     {
//         return Assertions.Expect(response);
//     }


//     private readonly List<IBrowserContext> _contexts = [];
//     public IBrowser Browser { get; internal set; }

//     public async Task<IBrowserContext> NewContext(BrowserNewContextOptions? options = null)
//     {
//         IBrowserContext browserContext = await Browser.NewContextAsync(options).ConfigureAwait(continueOnCapturedContext: false);
//         _contexts.Add(browserContext);
//         return browserContext;
//     }

//     [OneTimeSetUp]
//     public async Task BrowserSetup()
//     {
//         _playwrightTask = Microsoft.Playwright.Playwright.CreateAsync();
//         await PlaywrightSetup();
//         Browser = (await BrowserService.Register(this, BrowserType).ConfigureAwait(continueOnCapturedContext: false)).Browser;
//     }

//     [OneTimeTearDown]
//     public async Task BrowserTearDown()
//     {
//         _playwrightTask.Dispose();
//         if (TestOk())
//         {
//             foreach (IBrowserContext context in _contexts)
//             {
//                 await context.CloseAsync().ConfigureAwait(continueOnCapturedContext: false);
//             }
//         }

//         _contexts.Clear();
//         Browser = null;
//     }

//     public virtual BrowserNewContextOptions ContextOptions()
//     {
//         return new BrowserNewContextOptions
//         {
//             Locale = "pt-BR",
//             ColorScheme = ColorScheme.Dark,
//         };
//     }

//     public async Task<IPage> GetNewPage()
//     {
//         var context = await NewContext(ContextOptions()).ConfigureAwait(continueOnCapturedContext: false);
//         return await context.NewPageAsync().ConfigureAwait(continueOnCapturedContext: false);
//     }
// }

// internal class BrowserService : IWorkerService
// {
//     public IBrowser Browser { get; private set; }

//     private BrowserService(IBrowser browser)
//     {
//         Browser = browser;
//     }

//     public static Task<BrowserService> Register(SykiWorkerAwareTest test, IBrowserType browserType)
//     {
//         IBrowserType browserType2 = browserType;
//         return test.RegisterService("Browser", async () => new BrowserService(await CreateBrowser(browserType2).ConfigureAwait(continueOnCapturedContext: false)));
//     }

//     private static async Task<IBrowser> CreateBrowser(IBrowserType browserType)
//     {
//         string environmentVariable = Environment.GetEnvironmentVariable("PLAYWRIGHT_SERVICE_ACCESS_TOKEN") ?? "";
//         string environmentVariable2 = Environment.GetEnvironmentVariable("PLAYWRIGHT_SERVICE_URL") ?? "";
//         if (string.IsNullOrEmpty(environmentVariable) || string.IsNullOrEmpty(environmentVariable2))
//         {
//             return await browserType.LaunchAsync(PlaywrightSettingsProvider.LaunchOptions).ConfigureAwait(continueOnCapturedContext: false);
//         }

//         string exposeNetwork = Environment.GetEnvironmentVariable("PLAYWRIGHT_SERVICE_EXPOSE_NETWORK") ?? "<loopback>";
//         Dictionary<string, string> value = new Dictionary<string, string>
//         {
//             ["os"] = Environment.GetEnvironmentVariable("PLAYWRIGHT_SERVICE_OS") ?? "linux",
//             ["runId"] = Environment.GetEnvironmentVariable("PLAYWRIGHT_SERVICE_RUN_ID") ?? DateTime.Now.ToUniversalTime().ToString("yyyy-MM-ddTHH:mm:ss", CultureInfo.InvariantCulture)
//         };
//         string wsEndpoint = environmentVariable2 + "?cap=" + JsonSerializer.Serialize(value);
//         BrowserTypeConnectOptions options = new BrowserTypeConnectOptions
//         {
//             Timeout = 180000f,
//             ExposeNetwork = exposeNetwork,
//             Headers = new Dictionary<string, string> { ["x-mpt-access-key"] = environmentVariable }
//         };
//         return await browserType.ConnectAsync(wsEndpoint, options).ConfigureAwait(continueOnCapturedContext: false);
//     }

//     public Task ResetAsync()
//     {
//         return Task.CompletedTask;
//     }

//     public Task DisposeAsync()
//     {
//         return Browser.CloseAsync();
//     }
// }

// public class SykiWorkerAwareTest
// {
//     internal class Worker
//     {
//         private static int _lastWorkedIndex;

//         public int WorkerIndex = Interlocked.Increment(ref _lastWorkedIndex);

//         public Dictionary<string, IWorkerService> Services = new Dictionary<string, IWorkerService>();
//     }

//     private static readonly ConcurrentStack<Worker> _allWorkers = new ConcurrentStack<Worker>();

//     private Worker _currentWorker;

//     public int WorkerIndex { get; internal set; }

//     public async Task<T> RegisterService<T>(string name, Func<Task<T>> factory) where T : class, IWorkerService
//     {
//         if (!_currentWorker.Services.ContainsKey(name))
//         {
//             Dictionary<string, IWorkerService> services = _currentWorker.Services;
//             services[name] = await factory().ConfigureAwait(continueOnCapturedContext: false);
//         }

//         return _currentWorker.Services[name] as T;
//     }

//     [OneTimeSetUp]
//     public void WorkerSetup()
//     {
//         if (!_allWorkers.TryPop(out _currentWorker))
//         {
//             _currentWorker = new Worker();
//         }

//         WorkerIndex = _currentWorker.WorkerIndex;
//         if (PlaywrightSettingsProvider.ExpectTimeout.HasValue)
//         {
//             // AssertionsBase.SetDefaultTimeout(PlaywrightSettingsProvider.ExpectTimeout.Value);
//         }
//     }

//     [OneTimeTearDown]
//     public async Task WorkerTeardown()
//     {
//         if (TestOk())
//         {
//             foreach (KeyValuePair<string, IWorkerService> service in _currentWorker.Services)
//             {
//                 await service.Value.ResetAsync().ConfigureAwait(continueOnCapturedContext: false);
//             }

//             _allWorkers.Push(_currentWorker);
//             return;
//         }

//         foreach (KeyValuePair<string, IWorkerService> service2 in _currentWorker.Services)
//         {
//             await service2.Value.DisposeAsync().ConfigureAwait(continueOnCapturedContext: false);
//         }

//         _currentWorker.Services.Clear();
//     }

//     public bool TestOk()
//     {
//         if (TestContext.CurrentContext.Result.Outcome.Status != TestStatus.Passed)
//         {
//             return TestContext.CurrentContext.Result.Outcome.Status == TestStatus.Skipped;
//         }

//         return true;
//     }
// }
