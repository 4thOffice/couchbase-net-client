﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Enyim.Caching.Memcached;

namespace Couchbase
{
	public interface ICouchbaseOperationFactory : IOperationFactory
	{
		ITouchOperation Touch(string key, uint newExpiration);
		IGetAndTouchOperation GetAndTouch(string key, uint newExpiration);
		ISyncOperation Sync(SyncMode mode, IList<KeyValuePair<string, ulong>> keys, int replicationCount);
	}
}
