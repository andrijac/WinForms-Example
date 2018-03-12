using System;
using System.ComponentModel;
using System.Linq.Expressions;
using System.Reflection;

namespace WindowsFormsApplication1
{
	public class Presenter : INotifyPropertyChanged
	{
		#region INotifyPropertyChanged Members

		public event PropertyChangedEventHandler PropertyChanged;

		#endregion INotifyPropertyChanged Members

		private string name;

		private string secondName;

		private int width;

		protected virtual void OnPropertyChange(string propertyName)
		{
			if (this.PropertyChanged != null)
			{
				this.PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
			}
		}

		public string Name
		{
			get
			{
				return this.name;
			}
			set
			{
				this.name = value;
				this.OnPropertyChange(this.GetPropertyName(() => this.Name));
			}
		}

		public string SecondName
		{
			get
			{
				return this.secondName;
			}
			set
			{
				this.secondName = value;
				this.OnPropertyChange(this.GetPropertyName(() => this.SecondName));
			}
		}

		public int FormWidth
		{
			get
			{
				return this.width;
			}
			set
			{
				this.width = value;
				this.OnPropertyChange(this.GetPropertyName(() => this.FormWidth));
			}
		}

		#region TL;DR

		protected virtual string GetPropertyName<T>(Expression<Func<T>> propertyExpression)
		{
			return ExtractPropertyName(this, propertyExpression);
		}

		/// <summary>
		/// http://dotnet.dzone.com/articles/implementing
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <typeparam name="TObj"></typeparam>
		/// <param name="obj"></param>
		/// <param name="propertyExpression"></param>
		/// <returns></returns>
		public static string ExtractPropertyName<T, TObj>(TObj obj, Expression<Func<T>> propertyExpression)
		{
			if (propertyExpression == null)
			{
				throw new ArgumentNullException("propertyExpression");
			}

			// http://stackoverflow.com/questions/12975373/expression-for-type-members-results-in-different-expressions-memberexpression
			UnaryExpression unaryExpression = propertyExpression.Body as UnaryExpression;
			MemberExpression memberExpression = unaryExpression != null ?
												unaryExpression.Operand as MemberExpression :
												propertyExpression.Body as MemberExpression;

			if (memberExpression == null)
			{
				throw new ArgumentException("The expression is not a member access expression.", "propertyExpression");
			}

			PropertyInfo property = memberExpression.Member as PropertyInfo;
			if (property == null)
			{
				throw new ArgumentException("The member access expression does not access a property.", "propertyExpression");
			}

			if (!property.DeclaringType.IsAssignableFrom(obj.GetType()))
			{
				throw new ArgumentException("The referenced property belongs to a different type.", "propertyExpression");
			}

			MethodInfo getMethod = property.GetGetMethod(true);
			if (getMethod == null)
			{
				// this shouldn't happen - the expression would reject the property before reaching this far
				throw new ArgumentException("The referenced property does not have a get method.", "propertyExpression");
			}

			if (getMethod.IsStatic)
			{
				throw new ArgumentException("The referenced property is a static property.", "propertyExpression");
			}

			return memberExpression.Member.Name;
		}

		#endregion TL;DR
	}
}