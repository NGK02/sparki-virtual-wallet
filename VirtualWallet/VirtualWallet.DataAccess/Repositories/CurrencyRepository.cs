﻿using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VirtualWallet.DataAccess.Enums;
using VirtualWallet.DataAccess.Models;
using VirtualWallet.DataAccess.Repositories.Contracts;

namespace VirtualWallet.DataAccess.Repositories
{
    public class CurrencyRepository : ICurrencyRepository
    {
        private readonly WalletDbContext walletDbContext;

        public CurrencyRepository(WalletDbContext walletDbContext)
        {
            this.walletDbContext = walletDbContext;
        }

        public Currency GetCurrencyByCode(CurrencyCode currencyCode)
        {
            return walletDbContext.Currencies.Include(c => c.Code).SingleOrDefault(c => c.Code == currencyCode);
        }

        public Currency GetCurrencyById(int currencyId)
        {
            return walletDbContext.Currencies.Include(c => c.Code).SingleOrDefault(c => c.Id == currencyId);
        }
    }
}