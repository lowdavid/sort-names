using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using Common.Logging;

namespace Sort.Tests {
	public interface ITester {
		Type baseType { get; set; }
	}

	public static class TestExtensions {
		private static readonly ILog log = LogManager.GetLogger( System.Reflection.MethodBase.GetCurrentMethod().DeclaringType );

		public static object getField( this ITester obj, string name ) {
			FieldInfo f = obj.baseType.GetField( name, BindingFlags.Instance | BindingFlags.NonPublic );
			return f == null ? null : f.GetValue( obj );
		}

		public static void setField( this ITester obj, string name, object value ) {
			FieldInfo f = obj.baseType.GetField( name, BindingFlags.Instance | BindingFlags.NonPublic );
			if ( f != null ) {
				f.SetValue( obj, value );
			}
		}

		public static object executeMethod( this ITester obj, string name, params object[] parameters ) {
			MethodInfo m;
			if ( parameters == null || parameters.Length == 0 ) {
				m = obj.baseType.GetMethod( name, BindingFlags.Instance | BindingFlags.NonPublic );
			} else {
				Type[] types = new Type[parameters.Length];
				for ( int i = 0; i < parameters.Length; i++ ) {
					types[i] = parameters[i] == null ? typeof( object ) : parameters[i].GetType();
				}
				m = obj.baseType.GetMethod( name, BindingFlags.Instance | BindingFlags.NonPublic, null, types, null );
			}
			return m == null ? null : m.Invoke( obj, parameters );
		}
	}
}
