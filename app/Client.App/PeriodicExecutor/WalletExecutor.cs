using Client.App.Infrastructure.Managers;
using Client.App.Services;
using Client.Infrastructure.Constants;
using System;
using System.Diagnostics;
using System.Linq;
using System.Timers;

namespace Client.App.PeriodicExecutor
{
    public class WalletExecutor : IDisposable
    {
        private readonly IWalletManager _walletManager;
        private readonly IExceptionHandler _exceptionHandler;

        public WalletExecutor(IWalletManager walletManager, IExceptionHandler exceptionHandler)
        {
            _walletManager = walletManager;
            _exceptionHandler = exceptionHandler;
        }

        public event EventHandler<WalletJobExecutedEventArgs> JobExecuted;

        Timer _Timer;
        bool _Running;

        public void StartExecuting()
        {
            if (!_Running)
            {
                // Initiate a Timer
                _Timer = new Timer();
                _Timer.Interval = 30000;  // every 30 seconds
                _Timer.Elapsed += HandleTimer;
                _Timer.AutoReset = true;
                _Timer.Enabled = true;

                _Running = true;
            }
        }
        async void HandleTimer(object source, ElapsedEventArgs e)
        {
            try
            {
                await _exceptionHandler.HandlerRequestTaskAsync(() => _walletManager.GetAccountInfoAsync());
                var accountInfo = await _walletManager.FetchAccountInfoAsync();

                if (accountInfo != null)
                {
                    var walletAddress = await _walletManager.GetWalletAddressAsync();
                    if (walletAddress != null)
                    {
                        var balance = $"{accountInfo.Addresses.First(x => x.Address == walletAddress).AmountConfirmed / ClientConstants.SatoshiDivider} TCRS";

                        JobExecuted?.Invoke(this, new WalletJobExecutedEventArgs()
                        {
                            WalletName = accountInfo.WalletName,
                            WalletAddress = walletAddress,
                            WalletBalance = balance
                        });

                        Console.WriteLine($"Fetch Success {balance} {JobExecuted == null}");
                    }
                }

            }
            catch
            {
                Console.WriteLine("Fetch Error");
            }
        }

        public void Dispose()
        {
            if (_Running)
            {
                // Clear up the timer
            }
        }
    }

    public class WalletJobExecutedEventArgs : EventArgs
    {
        public string WalletName { get; set; }
        public string WalletAddress { get; set; }
        public string WalletBalance { get; set; }

    }

}
