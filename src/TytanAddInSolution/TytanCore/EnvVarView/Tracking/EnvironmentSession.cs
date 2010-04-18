using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;

namespace Pretorianie.Tytan.Core.EnvVarView.Tracking
{
    /// <summary>
    /// Session of environment variables changes.
    /// </summary>
    public class EnvironmentSession
    {
        /// <summary>
        /// Event fired each time new item is added or changed inside the collection.
        /// </summary>
        public event EnvironmentSessionChangedHandler Changed;

        private readonly List<EnvironmentSessionItem> _items = new List<EnvironmentSessionItem>();

        /// <summary>
        /// Default constructor.
        /// </summary>
        public EnvironmentSession ()
        {
        }

        /// <summary>
        /// Init constructor.
        /// </summary>
        public EnvironmentSession (string name)
        {
            Name = name;
        }

        /// <summary>
        /// Init constructor.
        /// </summary>
        public EnvironmentSession (string name, IEnumerable<EnvironmentSessionItem> items)
        {
            Name = name;
            _items = new List<EnvironmentSessionItem>(items);
        }

        /// <summary>
        /// The name of current environment session.
        /// </summary>
        public string Name
        { get; set; }

        /// <summary>
        /// Gets the location on the disk, where the session is stored (if is loaded from local file.
        /// </summary>
        public string FileName
        { get; private set; }

        /// <summary>
        /// Gets the items stored in the given session.
        /// </summary>
        public IEnumerable<EnvironmentSessionItem> Items
        {
            get { return _items; }
        }

        /// <summary>
        /// Gets the number of items.
        /// </summary>
        public int Count
        {
            get { return _items.Count; }
        }

        /// <summary>
        /// Gets the item at particular index.
        /// </summary>
        public EnvironmentSessionItem this[int index]
        {
            get { return _items[index]; }
        }

        /// <summary>
        /// Gets the item with particular name.
        /// </summary>
        public EnvironmentSessionItem this[string name]
        {
            get
            {
                foreach (EnvironmentSessionItem i in _items)
                    if (string.Compare(i.Name, name, true) == 0)
                        return i;

                return null;
            }
        }

        /// <summary>
        /// Gets the variable associated with given target and with specific name.
        /// </summary>
        public EnvironmentSessionItem GetVariable(EnvironmentVariableTarget target, string name)
        {
            if (string.IsNullOrEmpty(name))
                return null;

            foreach(EnvironmentSessionItem i in _items)
                if (target == i.Target
                    && string.Compare(i.Name, name, true) == 0)
                    return i;


            return null;
        }

        /// <summary>
        /// Adds, deletes or updates with new variable change request data inside the current session.
        /// </summary>
        public void Update(EnvironmentSessionItem item)
        {
            if (item == null)
                throw new ArgumentNullException("item");

            if (item.Target != EnvironmentVariableTarget.Process)
                Update(new EnvironmentSessionItem(EnvironmentVariableTarget.Process, item.Name, item.Value, item.PrimaryValue));

            EnvironmentSessionItem existingItem = GetVariable(item.Target, item.Name);
            bool notify = false;

            if (existingItem != null)
            {
                // if new value set to environment variable
                // is the same as the original value,
                // then we can remove it:
                if (existingItem.PrimaryValue == item.Value)
                {
                    _items.Remove(existingItem);
                    notify = true;
                }
                else
                {
                    if (existingItem.Value != item.Value)
                    {
                        existingItem.Value = item.Value;
                        notify = true;
                    }
                }
            }
            else
            {
                _items.Add(item);
                notify = true;
            }

            if (notify && Changed != null)
                Changed(this, new EnvironmentSessionEventArgs(_items.Count));
        }

        /// <summary>
        /// Adds, deletes or updates with new variable change request data inside the current session.
        /// </summary>
        public void Update (string name, string value)
        {
            Update(new EnvironmentSessionItem(EnvironmentVariableTarget.Process, name, value));
        }

        /// <summary>
        /// Adds, deletes or updates with new variable change request data inside the current session.
        /// </summary>
        public void Update(string name, string value, string primaryValue)
        {
            Update(new EnvironmentSessionItem(EnvironmentVariableTarget.Process, name, value, primaryValue));
        }

        /// <summary>
        /// Adds, deletes or updates with new variable change request data inside the current session.
        /// </summary>
        public void Update (EnvironmentVariableTarget target, string name, string value)
        {
            Update(new EnvironmentSessionItem(target, name, value));
        }

        /// <summary>
        /// Adds, deletes or updates with new variable change request data inside the current session.
        /// </summary>
        public void Update(EnvironmentVariableTarget target, string name, string value, string primaryValue)
        {
            Update(new EnvironmentSessionItem(target, name, value, primaryValue));
        }

        /// <summary>
        /// Adds, deletes or updates with new variable change request data inside the current session.
        /// </summary>
        public void Update(EnvironmentVariableTarget target, EnvironmentSessionItem item, EnvironmentVariable variable)
        {
            if (variable != null)
                Update(target, item.Name, item.Value, variable.Value);
            else
                Update(target, item.Name, item.Value, null);
        }

        /// <summary>
        /// Removes stored info about environment variable change.
        /// </summary>
        public void Remove(string name)
        {
            EnvironmentSessionItem existingItem = this[name];

            if (existingItem != null)
            {
                _items.Remove(existingItem);
                if (Changed != null)
                    Changed(this, new EnvironmentSessionEventArgs(_items.Count));
            }
        }

        /// <summary>
        /// Removes all monitored changes introduced in current session.
        /// </summary>
        public void RemoveAll()
        {
            if (_items.Count > 0)
            {
                _items.Clear();

                if (Changed != null)
                    Changed(this, new EnvironmentSessionEventArgs(0));
            }
        }

        /// <summary>
        /// Removes all monitored changes introduced in current session.
        /// </summary>
        public void Clear()
        {
            RemoveAll();
        }

        /// <summary>
        /// Reverts changes introduced in this session.
        /// </summary>
        public void Revert()
        {
            if (_items.Count > 0)
            {
                foreach (EnvironmentSessionItem item in _items)
                {
                    item.Revert();
                }
                _items.Clear();

                if (Changed != null)
                    Changed(this, new EnvironmentSessionEventArgs(0));
            }
        }

        /// <summary>
        /// Writes current session into a dedicated <see cref="StringWriter"/>.
        /// </summary>
        public void WriteTo(StreamWriter output)
        {
            output.WriteLine("[" + Name + "]");

            foreach (EnvironmentSessionItem item in _items)
            {
                output.WriteLine("{0}={1}", item.Name, item.Value);
            }
        }

        /// <summary>
        /// Loads session data from dedicated <see cref="StringReader"/>.
        /// </summary>
        public void LoadFrom(StreamReader input, string fileName)
        {
            string line;
            int index;

            Name = null;
            FileName = fileName;
            _items.Clear();

            while ((line = input.ReadLine()) != null)
            {
                // remove spaces from beginning and the end,
                // if the item is not a variable definition:
                index = line.IndexOf('=');
                if (index < 0)
                    line = line.Trim();

                if (!string.IsNullOrEmpty(line) && line.Length > 1)
                {
                    // is it the section header?:
                    if (line[0] == '[' && line[line.Length - 1] == ']')
                    {
                        Name = line.Substring(1, line.Length - 2);
                    }
                    else
                    {
                        // is it the <name>=<value> element?
                        if (index > 0)
                        {
                            string name = line.Substring(0, index);
                            string value = line.Substring(index + 1);
                            EnvironmentSessionItem item = new EnvironmentSessionItem(EnvironmentVariableTarget.Process,
                                                                                     name, value, value);

                            _items.Add(item);
                        }
                        else
                        {
                            // is this only a value written in the line?
                            EnvironmentSessionItem item = new EnvironmentSessionItem(EnvironmentVariableTarget.Process,
                                                                                     line, null, null);

                            _items.Add(item);
                        }
                    }
                }
                else
                {
                    Trace.WriteLine("Invalid line: '" + line + "'.");
                }
            }

            if (Changed != null)
                Changed(this, new EnvironmentSessionEventArgs(_items.Count));
        }
    }
}
