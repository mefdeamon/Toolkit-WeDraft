using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Windows.Input;

namespace FirstDraft.Mvvms
{
    /// <summary>
    /// A base class for objects of which the properties must be observable.
    /// </summary>
    public abstract class ObservableObject : INotifyPropertyChanged, INotifyPropertyChanging
    {
        /// <inheritdoc cref="INotifyPropertyChanged.PropertyChanged"/>
        public event PropertyChangedEventHandler PropertyChanged;

        /// <inheritdoc cref="INotifyPropertyChanging.PropertyChanging"/>
        public event PropertyChangingEventHandler PropertyChanging;

        /// <summary>
        /// Raises the <see cref="PropertyChanged"/> event.
        /// </summary>
        /// <param name="e">The input <see cref="PropertyChangedEventArgs"/> instance.</param>
        /// <exception cref="System.ArgumentNullException">Thrown if <paramref name="e"/> is <see langword="null"/>.</exception>
        protected virtual void OnPropertyChanged(PropertyChangedEventArgs e)
        {
            PropertyChanged?.Invoke(this, e);
        }

        /// <summary>
        /// Raises the <see cref="PropertyChanging"/> event.
        /// </summary>
        /// <param name="e">The input <see cref="PropertyChangingEventArgs"/> instance.</param>
        /// <exception cref="System.ArgumentNullException">Thrown if <paramref name="e"/> is <see langword="null"/>.</exception>
        protected virtual void OnPropertyChanging(PropertyChangingEventArgs e)
        {
            // When support is disabled, just do nothing
            if (!FeatureSwitches.EnableINotifyPropertyChangingSupport)
            {
                return;
            }

            PropertyChanging?.Invoke(this, e);
        }

        /// <summary>
        /// Raises the <see cref="PropertyChanged"/> event.
        /// </summary>
        /// <param name="propertyName">(optional) The name of the property that changed.</param>
        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            OnPropertyChanged(new PropertyChangedEventArgs(propertyName));
        }

        /// <summary>
        /// Raises the <see cref="PropertyChanging"/> event.
        /// </summary>
        /// <param name="propertyName">(optional) The name of the property that changed.</param>
        protected void OnPropertyChanging([CallerMemberName] string propertyName = null)
        {
            // When support is disabled, avoid instantiating the event args entirely
            if (!FeatureSwitches.EnableINotifyPropertyChangingSupport)
            {
                return;
            }

            OnPropertyChanging(new PropertyChangingEventArgs(propertyName));
        }

        /// <summary>
        /// Compares the current and new values for a given property. If the value has changed,
        /// raises the <see cref="PropertyChanging"/> event, updates the property with the new
        /// value, then raises the <see cref="PropertyChanged"/> event.
        /// </summary>
        /// <typeparam name="T">The type of the property that changed.</typeparam>
        /// <param name="field">The field storing the property's value.</param>
        /// <param name="newValue">The property's value after the change occurred.</param>
        /// <param name="propertyName">(optional) The name of the property that changed.</param>
        /// <returns><see langword="true"/> if the property was changed, <see langword="false"/> otherwise.</returns>
        /// <remarks>
        /// The <see cref="PropertyChanging"/> and <see cref="PropertyChanged"/> events are not raised
        /// if the current and new value for the target property are the same.
        /// </remarks>
        protected bool SetProperty<T>(ref T field, T newValue, [CallerMemberName] string propertyName = null)
        {
            // We duplicate the code here instead of calling the overload because we can't
            // guarantee that the invoked SetProperty<T> will be inlined, and we need the JIT
            // to be able to see the full EqualityComparer<T>.Default.Equals call, so that
            // it'll use the intrinsics version of it and just replace the whole invocation
            // with a direct comparison when possible (eg. for primitive numeric types).
            // This is the fastest SetProperty<T> overload so we particularly care about
            // the codegen quality here, and the code is small and simple enough so that
            // duplicating it still doesn't make the whole class harder to maintain.
            if (EqualityComparer<T>.Default.Equals(field, newValue))
            {
                return false;
            }

            OnPropertyChanging(propertyName);

            field = newValue;

            OnPropertyChanged(propertyName);

            return true;
        }

        /// <summary>
        /// Compares the current and new values for a given property. If the value has changed,
        /// raises the <see cref="PropertyChanging"/> event, updates the property with the new
        /// value, then raises the <see cref="PropertyChanged"/> event.
        /// See additional notes about this overload in <see cref="SetProperty{T}(ref T,T,string)"/>.
        /// </summary>
        /// <typeparam name="T">The type of the property that changed.</typeparam>
        /// <param name="field">The field storing the property's value.</param>
        /// <param name="newValue">The property's value after the change occurred.</param>
        /// <param name="comparer">The <see cref="IEqualityComparer{T}"/> instance to use to compare the input values.</param>
        /// <param name="propertyName">(optional) The name of the property that changed.</param>
        /// <returns><see langword="true"/> if the property was changed, <see langword="false"/> otherwise.</returns>
        /// <exception cref="System.ArgumentNullException">Thrown if <paramref name="comparer"/> is <see langword="null"/>.</exception>
        protected bool SetProperty<T>(ref T field, T newValue, IEqualityComparer<T> comparer, [CallerMemberName] string propertyName = null)
        {

            if (comparer.Equals(field, newValue))
            {
                return false;
            }

            OnPropertyChanging(propertyName);

            field = newValue;

            OnPropertyChanged(propertyName);

            return true;
        }

        /// <summary>
        /// Compares the current and new values for a given property. If the value has changed,
        /// raises the <see cref="PropertyChanging"/> event, updates the property with the new
        /// value, then raises the <see cref="PropertyChanged"/> event.
        /// This overload is much less efficient than <see cref="SetProperty{T}(ref T,T,string)"/> and it
        /// should only be used when the former is not viable (eg. when the target property being
        /// updated does not directly expose a backing field that can be passed by reference).
        /// For performance reasons, it is recommended to use a stateful callback if possible through
        /// the <see cref="SetProperty{TModel,T}(T,T,TModel,Action{TModel,T},string)"/> whenever possible
        /// instead of this overload, as that will allow the C# compiler to cache the input callback and
        /// reduce the memory allocations. More info on that overload are available in the related XML
        /// docs. This overload is here for completeness and in cases where that is not applicable.
        /// </summary>
        /// <typeparam name="T">The type of the property that changed.</typeparam>
        /// <param name="oldValue">The current property value.</param>
        /// <param name="newValue">The property's value after the change occurred.</param>
        /// <param name="callback">A callback to invoke to update the property value.</param>
        /// <param name="propertyName">(optional) The name of the property that changed.</param>
        /// <returns><see langword="true"/> if the property was changed, <see langword="false"/> otherwise.</returns>
        /// <remarks>
        /// The <see cref="PropertyChanging"/> and <see cref="PropertyChanged"/> events are not raised
        /// if the current and new value for the target property are the same.
        /// </remarks>
        /// <exception cref="System.ArgumentNullException">Thrown if <paramref name="callback"/> is <see langword="null"/>.</exception>
        protected bool SetProperty<T>(T oldValue, T newValue, Action<T> callback, [CallerMemberName] string propertyName = null)
        {

            // We avoid calling the overload again to ensure the comparison is inlined
            if (EqualityComparer<T>.Default.Equals(oldValue, newValue))
            {
                return false;
            }

            OnPropertyChanging(propertyName);

            callback(newValue);

            OnPropertyChanged(propertyName);

            return true;
        }

        /// <summary>
        /// Compares the current and new values for a given property. If the value has changed,
        /// raises the <see cref="PropertyChanging"/> event, updates the property with the new
        /// value, then raises the <see cref="PropertyChanged"/> event.
        /// See additional notes about this overload in <see cref="SetProperty{T}(T,T,Action{T},string)"/>.
        /// </summary>
        /// <typeparam name="T">The type of the property that changed.</typeparam>
        /// <param name="oldValue">The current property value.</param>
        /// <param name="newValue">The property's value after the change occurred.</param>
        /// <param name="comparer">The <see cref="IEqualityComparer{T}"/> instance to use to compare the input values.</param>
        /// <param name="callback">A callback to invoke to update the property value.</param>
        /// <param name="propertyName">(optional) The name of the property that changed.</param>
        /// <returns><see langword="true"/> if the property was changed, <see langword="false"/> otherwise.</returns>
        /// <exception cref="System.ArgumentNullException">Thrown if <paramref name="comparer"/> or <paramref name="callback"/> are <see langword="null"/>.</exception>
        protected bool SetProperty<T>(T oldValue, T newValue, IEqualityComparer<T> comparer, Action<T> callback, [CallerMemberName] string propertyName = null)
        {

            if (comparer.Equals(oldValue, newValue))
            {
                return false;
            }

            OnPropertyChanging(propertyName);

            callback(newValue);

            OnPropertyChanged(propertyName);

            return true;
        }

        /// <summary>
        /// Compares the current and new values for a given nested property. If the value has changed,
        /// raises the <see cref="PropertyChanging"/> event, updates the property and then raises the
        /// <see cref="PropertyChanged"/> event. The behavior mirrors that of <see cref="SetProperty{T}(ref T,T,string)"/>,
        /// with the difference being that this method is used to relay properties from a wrapped model in the
        /// current instance. This type is useful when creating wrapping, bindable objects that operate over
        /// models that lack support for notification (eg. for CRUD operations).
        /// Suppose we have this model (eg. for a database row in a table):
        /// <code>
        /// public class Person
        /// {
        ///     public string Name { get; set; }
        /// }
        /// </code>
        /// We can then use a property to wrap instances of this type into our observable model (which supports
        /// notifications), injecting the notification to the properties of that model, like so:
        /// <code>
        /// public class BindablePerson : ObservableObject
        /// {
        ///     public Model { get; }
        ///
        ///     public BindablePerson(Person model)
        ///     {
        ///         Model = model;
        ///     }
        ///
        ///     public string Name
        ///     {
        ///         get => Model.Name;
        ///         set => Set(Model.Name, value, Model, (model, name) => model.Name = name);
        ///     }
        /// }
        /// </code>
        /// This way we can then use the wrapping object in our application, and all those "proxy" properties will
        /// also raise notifications when changed. Note that this method is not meant to be a replacement for
        /// <see cref="SetProperty{T}(ref T,T,string)"/>, and it should only be used when relaying properties to a model that
        /// doesn't support notifications, and only if you can't implement notifications to that model directly (eg. by having
        /// it inherit from <see cref="ObservableObject"/>). The syntax relies on passing the target model and a stateless callback
        /// to allow the C# compiler to cache the function, which results in much better performance and no memory usage.
        /// </summary>
        /// <typeparam name="TModel">The type of model whose property (or field) to set.</typeparam>
        /// <typeparam name="T">The type of property (or field) to set.</typeparam>
        /// <param name="oldValue">The current property value.</param>
        /// <param name="newValue">The property's value after the change occurred.</param>
        /// <param name="model">The model containing the property being updated.</param>
        /// <param name="callback">The callback to invoke to set the target property value, if a change has occurred.</param>
        /// <param name="propertyName">(optional) The name of the property that changed.</param>
        /// <returns><see langword="true"/> if the property was changed, <see langword="false"/> otherwise.</returns>
        /// <remarks>
        /// The <see cref="PropertyChanging"/> and <see cref="PropertyChanged"/> events are not
        /// raised if the current and new value for the target property are the same.
        /// </remarks>
        /// <exception cref="System.ArgumentNullException">Thrown if <paramref name="model"/> or <paramref name="callback"/> are <see langword="null"/>.</exception>
        protected bool SetProperty<TModel, T>(T oldValue, T newValue, TModel model, Action<TModel, T> callback, [CallerMemberName] string propertyName = null)
            where TModel : class
        {

            if (EqualityComparer<T>.Default.Equals(oldValue, newValue))
            {
                return false;
            }

            OnPropertyChanging(propertyName);

            callback(model, newValue);

            OnPropertyChanged(propertyName);

            return true;
        }

        /// <summary>
        /// Compares the current and new values for a given nested property. If the value has changed,
        /// raises the <see cref="PropertyChanging"/> event, updates the property and then raises the
        /// <see cref="PropertyChanged"/> event. The behavior mirrors that of <see cref="SetProperty{T}(ref T,T,string)"/>,
        /// with the difference being that this method is used to relay properties from a wrapped model in the
        /// current instance. See additional notes about this overload in <see cref="SetProperty{TModel,T}(T,T,TModel,Action{TModel,T},string)"/>.
        /// </summary>
        /// <typeparam name="TModel">The type of model whose property (or field) to set.</typeparam>
        /// <typeparam name="T">The type of property (or field) to set.</typeparam>
        /// <param name="oldValue">The current property value.</param>
        /// <param name="newValue">The property's value after the change occurred.</param>
        /// <param name="comparer">The <see cref="IEqualityComparer{T}"/> instance to use to compare the input values.</param>
        /// <param name="model">The model containing the property being updated.</param>
        /// <param name="callback">The callback to invoke to set the target property value, if a change has occurred.</param>
        /// <param name="propertyName">(optional) The name of the property that changed.</param>
        /// <returns><see langword="true"/> if the property was changed, <see langword="false"/> otherwise.</returns>
        /// <exception cref="System.ArgumentNullException">Thrown if <paramref name="comparer"/>, <paramref name="model"/> or <paramref name="callback"/> are <see langword="null"/>.</exception>
        protected bool SetProperty<TModel, T>(T oldValue, T newValue, IEqualityComparer<T> comparer, TModel model, Action<TModel, T> callback, [CallerMemberName] string propertyName = null)
            where TModel : class
        {
            if (comparer.Equals(oldValue, newValue))
            {
                return false;
            }

            OnPropertyChanging(propertyName);

            callback(model, newValue);

            OnPropertyChanged(propertyName);

            return true;
        }
        /// <summary>
        /// An interface for task notifiers of a specified type.
        /// </summary>
        /// <typeparam name="TTask">The type of value to store.</typeparam>
        private interface ITaskNotifier<TTask>
            where TTask : Task
        {
            /// <summary>
            /// Gets or sets the wrapped <typeparamref name="TTask"/> value.
            /// </summary>
            TTask Task { get; set; }
        }

        /// <summary>
        /// A wrapping class that can hold a <see cref="Task"/> value.
        /// </summary>
        protected sealed class TaskNotifier : ITaskNotifier<Task>
        {
            /// <summary>
            /// Initializes a new instance of the <see cref="TaskNotifier"/> class.
            /// </summary>
            internal TaskNotifier()
            {
            }

            private Task task;

            /// <inheritdoc/>
            Task ITaskNotifier<Task>.Task
            {
                get => this.task;
                set => this.task = value;
            }

            /// <summary>
            /// Unwraps the <see cref="Task"/> value stored in the current instance.
            /// </summary>
            /// <param name="notifier">The input <see cref="TaskNotifier{TTask}"/> instance.</param>
            public static implicit operator Task(TaskNotifier notifier)
            {
                return notifier.task;
            }
        }

        /// <summary>
        /// A wrapping class that can hold a <see cref="Task{T}"/> value.
        /// </summary>
        /// <typeparam name="T">The type of value for the wrapped <see cref="Task{T}"/> instance.</typeparam>
        protected sealed class TaskNotifier<T> : ITaskNotifier<Task<T>>
        {
            /// <summary>
            /// Initializes a new instance of the <see cref="TaskNotifier{TTask}"/> class.
            /// </summary>
            internal TaskNotifier()
            {
            }

            private Task<T> task;

            /// <inheritdoc/>
            Task<T> ITaskNotifier<Task<T>>.Task
            {
                get => this.task;
                set => this.task = value;
            }

            /// <summary>
            /// Unwraps the <see cref="Task{T}"/> value stored in the current instance.
            /// </summary>
            /// <param name="notifier">The input <see cref="TaskNotifier{TTask}"/> instance.</param>
            public static implicit operator Task<T>(TaskNotifier<T> notifier)
            {
                return notifier.task;
            }
        }
    }

    /// <summary>
    /// A command whose sole purpose is to relay its functionality to other
    /// objects by invoking delegates. The default return value for the <see cref="CanExecute"/>
    /// method is <see langword="true"/>. This type does not allow you to accept command parameters
    /// in the <see cref="Execute"/> and <see cref="CanExecute"/> callback methods.
    /// </summary>
    public sealed partial class RelayCommand : IRelayCommand
    {
        /// <summary>
        /// The <see cref="Action"/> to invoke when <see cref="Execute"/> is used.
        /// </summary>
        private readonly Action execute;

        /// <summary>
        /// The optional action to invoke when <see cref="CanExecute"/> is used.
        /// </summary>
        private readonly Func<bool> canExecute;

        /// <inheritdoc/>
        public event EventHandler CanExecuteChanged;

        /// <summary>
        /// Initializes a new instance of the <see cref="RelayCommand"/> class that can always execute.
        /// </summary>
        /// <param name="execute">The execution logic.</param>
        /// <exception cref="System.ArgumentNullException">Thrown if <paramref name="execute"/> is <see langword="null"/>.</exception>
        public RelayCommand(Action execute)
        {
            this.execute = execute;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="RelayCommand"/> class.
        /// </summary>
        /// <param name="execute">The execution logic.</param>
        /// <param name="canExecute">The execution status logic.</param>
        /// <exception cref="System.ArgumentNullException">Thrown if <paramref name="execute"/> or <paramref name="canExecute"/> are <see langword="null"/>.</exception>
        public RelayCommand(Action execute, Func<bool> canExecute)
        {
            this.execute = execute;
            this.canExecute = canExecute;
        }

        /// <inheritdoc/>
        public void NotifyCanExecuteChanged()
        {
            CanExecuteChanged.Invoke(this, EventArgs.Empty);
        }

        /// <inheritdoc/>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool CanExecute(object parameter)
        {
            if (this.canExecute == null) return true;
            return this.canExecute.Invoke() != false;
        }

        /// <inheritdoc/>
        public void Execute(object parameter)
        {
            this.execute();
        }
    }

    /// <summary>
    /// A generic command whose sole purpose is to relay its functionality to other
    /// objects by invoking delegates. The default return value for the CanExecute
    /// method is <see langword="true"/>. This class allows you to accept command parameters
    /// in the <see cref="Execute(T)"/> and <see cref="CanExecute(T)"/> callback methods.
    /// </summary>
    /// <typeparam name="T">The type of parameter being passed as input to the callbacks.</typeparam>
    public sealed partial class RelayCommand<T> : IRelayCommand<T>
    {
        /// <summary>
        /// The <see cref="Action"/> to invoke when <see cref="Execute(T)"/> is used.
        /// </summary>
        private readonly Action<T> execute;

        /// <summary>
        /// The optional action to invoke when <see cref="CanExecute(T)"/> is used.
        /// </summary>
        private readonly Predicate<T> canExecute;

        /// <inheritdoc/>
        public event EventHandler CanExecuteChanged;

        /// <summary>
        /// Initializes a new instance of the <see cref="RelayCommand{T}"/> class that can always execute.
        /// </summary>
        /// <param name="execute">The execution logic.</param>
        /// <remarks>
        /// Due to the fact that the <see cref="System.Windows.Input.ICommand"/> interface exposes methods that accept a
        /// nullable <see cref="object"/> parameter, it is recommended that if <typeparamref name="T"/> is a reference type,
        /// you should always declare it as nullable, and to always perform checks within <paramref name="execute"/>.
        /// </remarks>
        /// <exception cref="System.ArgumentNullException">Thrown if <paramref name="execute"/> is <see langword="null"/>.</exception>
        public RelayCommand(Action<T> execute)
        {
            this.execute = execute;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="RelayCommand{T}"/> class.
        /// </summary>
        /// <param name="execute">The execution logic.</param>
        /// <param name="canExecute">The execution status logic.</param>
        /// <remarks>See notes in <see cref="RelayCommand{T}(Action{T})"/>.</remarks>
        /// <exception cref="System.ArgumentNullException">Thrown if <paramref name="execute"/> or <paramref name="canExecute"/> are <see langword="null"/>.</exception>
        public RelayCommand(Action<T> execute, Predicate<T> canExecute)
        {
            this.execute = execute;
            this.canExecute = canExecute;
        }

        /// <inheritdoc/>
        public void NotifyCanExecuteChanged()
        {
            CanExecuteChanged.Invoke(this, EventArgs.Empty);
        }

        /// <inheritdoc/>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool CanExecute(T parameter)
        {
            return this.canExecute.Invoke(parameter) != false;
        }

        /// <inheritdoc/>
        public bool CanExecute(object parameter)
        {
            // Special case a null value for a value type argument type.
            // This ensures that no exceptions are thrown during initialization.
            if (parameter is null && default(T) != null)
            {
                return false;
            }

            if (!TryGetCommandArgument(parameter, out T result))
            {
                throw new Exception(parameter.ToString());
            }

            return CanExecute(result);
        }

        /// <inheritdoc/>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Execute(T parameter)
        {
            this.execute(parameter);
        }

        /// <inheritdoc/>
        public void Execute(object parameter)
        {
            if (!TryGetCommandArgument(parameter, out T result))
            {
                throw new Exception(parameter.ToString());
            }

            Execute(result);
        }

        /// <summary>
        /// Tries to get a command argument of compatible type <typeparamref name="T"/> from an input <see cref="object"/>.
        /// </summary>
        /// <param name="parameter">The input parameter.</param>
        /// <param name="result">The resulting <typeparamref name="T"/> value, if any.</param>
        /// <returns>Whether or not a compatible command argument could be retrieved.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal static bool TryGetCommandArgument(object parameter, out T result)
        {
            // If the argument is null and the default value of T is also null, then the
            // argument is valid. T might be a reference type or a nullable value type.
            if (parameter is null && default(T) == null)
            {
                result = default;

                return true;
            }

            // Check if the argument is a T value, so either an instance of a type or a derived
            // type of T is a reference type, an interface implementation if T is an interface,
            // or a boxed value type in case T was a value type.
            if (parameter is T argument)
            {
                result = argument;

                return true;
            }

            result = default;

            return false;
        }

    }

    /// <summary>
    /// An interface expanding <see cref="ICommand"/> with the ability to raise
    /// the <see cref="ICommand.CanExecuteChanged"/> event externally.
    /// </summary>
    public interface IRelayCommand : ICommand
    {
        /// <summary>
        /// Notifies that the <see cref="ICommand.CanExecute"/> property has changed.
        /// </summary>
        void NotifyCanExecuteChanged();
    }
    /// <summary>
    /// A generic interface representing a more specific version of <see cref="IRelayCommand"/>.
    /// </summary>
    /// <typeparam name="T">The type used as argument for the interface methods.</typeparam>
    public interface IRelayCommand<in T> : IRelayCommand
    {
        /// <summary>
        /// Provides a strongly-typed variant of <see cref="ICommand.CanExecute(object)"/>.
        /// </summary>
        /// <param name="parameter">The input parameter.</param>
        /// <returns>Whether or not the current command can be executed.</returns>
        /// <remarks>Use this overload to avoid boxing, if <typeparamref name="T"/> is a value type.</remarks>
        bool CanExecute(T parameter);

        /// <summary>
        /// Provides a strongly-typed variant of <see cref="ICommand.Execute(object)"/>.
        /// </summary>
        /// <param name="parameter">The input parameter.</param>
        /// <remarks>Use this overload to avoid boxing, if <typeparamref name="T"/> is a value type.</remarks>
        void Execute(T parameter);
    }
    /// <summary>
    /// A container for all shared <see cref="AppContext"/> configuration switches for the MVVM Toolkit.
    /// </summary>
    /// <remarks>
    /// <para>
    /// This type uses a very specific setup for configuration switches to ensure ILLink can work the best.
    /// This mirrors the architecture of feature switches in the runtime as well, and it's needed so that
    /// no static constructor is generated for the type.
    /// </para>
    /// <para>
    /// For more info, see <see href="https://github.com/dotnet/runtime/blob/main/docs/workflow/trimming/feature-switches.md#adding-new-feature-switch"/>.
    /// </para>
    /// </remarks>
    internal static class FeatureSwitches
    {
        /// <summary>
        /// The configuration property name for <see cref="EnableINotifyPropertyChangingSupport"/>.
        /// </summary>
        private const string EnableINotifyPropertyChangingSupportPropertyName = "MVVMTOOLKIT_ENABLE_INOTIFYPROPERTYCHANGING_SUPPORT";

        /// <summary>
        /// The backing field for <see cref="EnableINotifyPropertyChangingSupport"/>.
        /// </summary>
        private static int enableINotifyPropertyChangingSupport;

        /// <summary>
        /// Gets a value indicating whether or not support for <see cref="System.ComponentModel.INotifyPropertyChanging"/> should be enabled (defaults to <see langword="true"/>).
        /// </summary>
        public static bool EnableINotifyPropertyChangingSupport
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => GetConfigurationValue(EnableINotifyPropertyChangingSupportPropertyName, ref enableINotifyPropertyChangingSupport, true);
        }

        /// <summary>
        /// Gets a configuration value for a specified property.
        /// </summary>
        /// <param name="propertyName">The property name to retrieve the value for.</param>
        /// <param name="cachedResult">The cached result for the target configuration value.</param>
        /// <param name="defaultValue">The default value for the feature switch, if not set.</param>
        /// <returns>The value of the specified configuration setting.</returns>
        private static bool GetConfigurationValue(string propertyName, ref int cachedResult, bool defaultValue)
        {
            // The cached switch value has 3 states:
            //   0: unknown.
            //   1: true
            //   -1: false
            //
            // This method doesn't need to worry about concurrent accesses to the cached result,
            // as even if the configuration value is retrieved twice, that'll always be the same.
            if (cachedResult < 0)
            {
                return false;
            }

            if (cachedResult > 0)
            {
                return true;
            }

            // Get the configuration switch value, or its default.
            // All feature switches have a default set in the .targets file.
            if (!AppContext.TryGetSwitch(propertyName, out bool isEnabled))
            {
                isEnabled = defaultValue;
            }

            // Update the cached result
            cachedResult = isEnabled ? 1 : -1;

            return isEnabled;
        }
    }

}
