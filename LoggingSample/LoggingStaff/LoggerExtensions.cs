using System;
using Microsoft.Extensions.Logging;

namespace LoggingSample.LoggingStaff
{
    public static class LoggerExtensions
    {
        // get method
        private static readonly Action<ILogger, Exception> _get;
        private static readonly Action<ILogger, int, Exception> _getItem;
        private static readonly Action<ILogger, int, Exception> _notFound;

        // post method
        private static readonly Action<ILogger, string, Exception> _postItem;

        // put method
        private static readonly Action<ILogger, string, string, int, Exception> _putItem;

        // delete method
        private static readonly Action<ILogger, string, int, Exception> _delItem;
        private static readonly Action<ILogger, int, Exception> _delItemFailed;


        // delete all, scope
        private static Func<ILogger, int, IDisposable> _delAllScope;
        // update with scope
        private static Func<ILogger, int, IDisposable> _updateWithScope;
        

        static LoggerExtensions()
        {
            
            _get = LoggerMessage.Define(
                LogLevel.Information, 
                new EventId(LoggingEvents.GetItems, nameof(Get)), 
                "GET request");
            
            _getItem = LoggerMessage.Define<int>(
                LogLevel.Information, 
                new EventId(LoggingEvents.GetItem, nameof(GetItem)), 
                "GET item request (Id = '{Id}')");

            _notFound = LoggerMessage.Define<int>(
                LogLevel.Warning, 
                new EventId(LoggingEvents.GetItemNotFound, nameof(NotFound)), 
                "Item not found (Id = '{Id}')");

            _postItem = LoggerMessage.Define<string>(
                LogLevel.Information, 
                new EventId(LoggingEvents.InsertItem, nameof(PostItem)), 
                "Item added (Item = '{Value}')");
            
            _putItem = LoggerMessage.Define<string, string, int>(
                LogLevel.Information, 
                new EventId(LoggingEvents.UpdateItem, nameof(PutItem)), 
                "Item changed (PrevItem = '{PrevItem}', NewItem = '{NewItem}' Id = {Id})");
            
            _delItem = LoggerMessage.Define<string, int>(
                LogLevel.Information, 
                new EventId(LoggingEvents.DeleteItem, nameof(DeleteItem)), 
                "Item deleted (Item = '{Value}' Id = {Id})");

            _delItemFailed = LoggerMessage.Define<int>(
                LogLevel.Error, 
                new EventId(LoggingEvents.DeleteItemFailed, nameof(DeleteItemFailed)), 
                "Item delete failed (Id = {Id})");
            
            _delAllScope = LoggerMessage.DefineScope<int>("All items deleted (Count = {Count})");
            _updateWithScope = LoggerMessage.DefineScope<int>("Update item (Id = {Id})");
        }

        #region get
        public static void Get(this ILogger logger)
        {
            _get(logger, null);
        }

        public static void GetItem(this ILogger logger, int id)
        {
            _getItem(logger, id, null);
        }

        public static void NotFound(this ILogger logger, int id)
        {
            _notFound(logger, id, null);
        }
        #endregion

        #region add item
        public static void PostItem(this ILogger logger, string quote)
        {
            _postItem(logger, quote, null);
        }
        #endregion

        #region update item
        public static void PutItem(this ILogger logger, int id, string priorItem, string newItem)
        {
            _putItem(logger, priorItem, newItem, id, null);
        }
        #endregion

        #region delete item
        public static void DeleteItem(this ILogger logger, string item, int id)
        {
            _delItem(logger, item, id, null);
        }

        public static void DeleteItemFailed(this ILogger logger, int id, Exception ex)
        {
            _delItemFailed(logger, id, ex);
        }
        #endregion


        #region scope operations
        public static IDisposable DeleteAllScope(this ILogger logger, int count)
        {
            return _delAllScope(logger, count);
        }

        public static IDisposable UpdateWithScope(this ILogger logger, int id)
        {
            return _updateWithScope(logger, id);
        }
        #endregion
    }
}