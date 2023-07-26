﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VirtualWallet.DataAccess.Enums;
using VirtualWallet.DataAccess.Models;

namespace VirtualWallet.Business.Services.Contracts
{
    public interface ICurrencyService
    {
        Currency GetCurrencyByCode(string currencyCode);

        Currency GetCurrencyById(int currencyId);
    }
}