using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VirtualWallet.DataAccess.Enums;
using VirtualWallet.DataAccess.Models;
using VirtualWallet.Dto.TransactionDto;
using VirtualWallet.Dto.UserDto;
using VirtualWallet.Business.Services;
using VirtualWallet.Business.Services.Contracts;
using VirtualWallet.Dto.CardDto;
using VirtualWallet.Dto.TransferDto;
using VirtualWallet.Dto.ExchangeDto;
using VirtualWallet.Dto.ViewModels.CardViewModels;
using VirtualWallet.Dto.ViewModels.TransferViewModels;
using VirtualWallet.Dto.ViewModels.UserViewModels;
using VirtualWallet.Dto.ViewModels.CurrencyViewModels;
using VirtualWallet.Dto.ViewModels.AdminViewModels;
using VirtualWallet.DataAccess.QueryParameters;
using VirtualWallet.Dto.ViewModels.ExchangeViewModels;
using VirtualWallet.Dto.ViewModels.WalletTransactionViewModels;

namespace VirtualWallet.Business.AutoMapper
{
    public class AutoMapperProfile : Profile
    {
        public const string dateFormat = "dd MMMM yyyy HH:mm:ss";

        public AutoMapperProfile()
        {

            CreateMap<Card, CardInfoDto>();
            CreateMap<CardInfoDto, Card>();

            CreateMap<CardInfoDto, Card>();

            CreateMap<CreateUserDto, User>();
            CreateMap<RegisterUser, User>();

            CreateMap<UpdateUserDto, User>();
            CreateMap<EditUser, User>();

            CreateMap<User, GetUserDto>()
                .ForMember(guDto => guDto.CardsCount, opt => opt.MapFrom(u => u.Cards.Count))
                .ForMember(guDto => guDto.Role, opt => opt.MapFrom(u => u.Role.Name.ToString()));

            CreateMap<User, GetUserViewModel>()
                .ForMember(guDto => guDto.CardsCount, opt => opt.MapFrom(u => u.Cards.Count))
                .ForMember(guDto => guDto.TransactionsCount, opt => opt.MapFrom(u => u.Outgoing.Count));



            CreateMap<CreateWalletTransactionDto, WalletTransaction>();
            CreateMap<WalletTransaction, GetWalletTransactionDto>()
                .ForMember(wtDto => wtDto.SenderUsername, opt => opt.MapFrom(wt => wt.Sender.Username))
                .ForMember(wtDto => wtDto.RecipientUsername, opt => opt.MapFrom(wt => wt.Recipient.Username))
                .ForMember(wtDto => wtDto.CurrencyCode, opt => opt.MapFrom(wt => wt.Currency.Code.ToString()))
                .ForMember(wtDto => wtDto.Amount, opt => opt.MapFrom(wt => wt.Amount));
            CreateMap<WalletTransaction, GetWalletTransactionViewModel>()
                .ForMember(wtDto => wtDto.SenderUsername, opt => opt.MapFrom(wt => wt.Sender.Username))
                .ForMember(wtDto => wtDto.RecipientUsername, opt => opt.MapFrom(wt => wt.Recipient.Username))
                .ForMember(wtDto => wtDto.CurrencyCode, opt => opt.MapFrom(wt => wt.Currency.Code.ToString()))
                .ForMember(wtDto => wtDto.Amount, opt => opt.MapFrom(wt => wt.Amount));

            CreateMap<CreateTransferDto, Transfer>();
            CreateMap<Transfer, GetTransferDto>()
                .ForMember(wtDto => wtDto.CurrencyCode, opt => opt.MapFrom(wt => wt.Currency.Code.ToString()));

            CreateMap<PaginatedTransfersViewModel, QueryParams>();
            CreateMap<Transfer, GetTransferViewModel>()
                .ForMember(dest => dest.Card, opt => opt.MapFrom(src => FormatCardNumber(src.Card.CardNumber)))
                .ForMember(dest => dest.Currency, opt => opt.MapFrom(src => src.Currency != null ? src.Currency.Code.ToString() : string.Empty))
                .ForMember(dest => dest.Date, opt => opt.MapFrom(src => src.CreatedOn.ToString("yyyy-MM-dd HH:mm:ss")))
                .ForMember(dest => dest.Username, opt => opt.MapFrom(src => src.Wallet.User != null ? src.Wallet.User.Username : string.Empty));

            CreateMap<Exchange, GetExchangeDto>()
                .ForMember(ExDto => ExDto.FromCurrency, opt => opt.MapFrom(e => e.FromCurrency.Code.ToString()))
                .ForMember(ExDto => ExDto.ToCurrency, opt => opt.MapFrom(e => e.ToCurrency.Code.ToString()));
            CreateMap<PaginateExchanges, QueryParams>();
            CreateMap<Exchange, GetExchangeViewModel>()
                .ForMember(ExDto => ExDto.Date, opt => opt.MapFrom(d => d.CreatedOn.ToString(dateFormat)))
                .ForMember(ExDto => ExDto.FromCurrency, opt => opt.MapFrom(e => e.FromCurrency.Code.ToString()))
                .ForMember(ExDto => ExDto.ToCurrency, opt => opt.MapFrom(e => e.ToCurrency.Code.ToString()));

            CreateMap<SearchUser, UserQueryParameters>() //Сравняване case insensitive.
                .ForMember(dest => dest.PhoneNumber, opt => opt.MapFrom(src => src.SearchOption == "Phonenumber" ? src.SearchOptionValue : null))
                .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.SearchOption == "Email" ? src.SearchOptionValue : null))
                .ForMember(dest => dest.Username, opt => opt.MapFrom(src => src.SearchOption == "Username" ? src.SearchOptionValue : null));

            CreateMap<Card, CardViewModel>()
                .ForMember(dest => dest.ExpirationMonth, opt => opt.MapFrom(src => src.ExpirationDate.ToString("MM")))
                .ForMember(dest => dest.ExpirationYear, opt => opt.MapFrom(src => src.ExpirationDate.ToString("yyyy")));

            CreateMap<CardViewModel, Card>()
                .ForMember(dest => dest.ExpirationDate, opt => opt.MapFrom(src => ParseDate(src.ExpirationMonth + "-" + src.ExpirationYear)));

            CreateMap<Card, SelectCardViewModel>();

            CreateMap<Card, GetCardViewModel>()
                .ForMember(dest => dest.ExpirationMonth, opt => opt.MapFrom(src => src.ExpirationDate.ToString("MM")))
                .ForMember(dest => dest.ExpirationYear, opt => opt.MapFrom(src => src.ExpirationDate.ToString("yyyy")));

            CreateMap<Transfer, TransferViewModel>();
            CreateMap<TransferViewModel, Transfer>();

            CreateMap<CreateWalletTransactionViewModel, WalletTransaction>()
                .ForMember(dest => dest.Recipient, opt => opt.MapFrom(src =>
                            src.RecipientIdentifier.Equals("Username", StringComparison.InvariantCultureIgnoreCase) ? new User { Username = src.RecipientIdentifierValue } :
                            src.RecipientIdentifier.Equals("Email", StringComparison.InvariantCultureIgnoreCase) ? new User { Email = src.RecipientIdentifierValue } :
                            src.RecipientIdentifier.Equals("Phonenumber", StringComparison.InvariantCultureIgnoreCase) ? new User { PhoneNumber = src.RecipientIdentifierValue } : null));

            CreateMap<Currency, CurrencyViewModel>();
            CreateMap<CurrencyViewModel, Currency>().ForMember(dest => dest.Id, opt => opt.Ignore());

            CreateMap<Balance, GetBalanceViewModel>();
        }

        private string FormatCardNumber(long cardNumber)
        {
            string cardNumberString = cardNumber.ToString();

            if (!string.IsNullOrEmpty(cardNumberString))
            {
                int chunkSize = 4;
                string formattedCardNumber = string.Join(" ", Enumerable.Range(0, cardNumberString.Length / chunkSize)
                    .Select(i => cardNumberString.Substring(i * chunkSize, chunkSize)));

                return formattedCardNumber;
            }

            return string.Empty;
        }

        private DateTime ParseDate(string date)
        {
            if (!DateTime.TryParseExact(date, "MM-yyyy", System.Globalization.CultureInfo.InvariantCulture, System.Globalization.DateTimeStyles.None, out DateTime expirationDate))
            {
                throw new ArgumentException("Invalid expiration date format.");
            }

            return expirationDate;
        }
    }
}