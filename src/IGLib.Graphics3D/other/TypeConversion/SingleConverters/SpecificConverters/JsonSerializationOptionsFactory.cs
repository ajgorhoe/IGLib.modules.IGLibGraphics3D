
#nullable disable

using Microsoft.Extensions.Options;
using System;
using System.Text.Json;
using System.Text.Json.Serialization;
using IGLib.Core.CollectionExtensions;

namespace IGLib.Core
{

    /// <summary>A factory that provides standard <see cref="JsonSerializerOptions"/> objects for
    /// non-polymorfic serialization.
    /// <para>Objects created by this factory should be compatible in the sense that any pair of these options
    /// should enable round-trip serialization / deserialization in the scope of the inherent limitations.</para>
    /// <para>The basic limitation is that the created serializer options do not enable polymorphic 
    /// serialization and deserialization.</para>
    /// <para>Other specifics:</para>
    /// <para>By default, object graph's maximum depth is 64, but this can be overridden by some factory mehods.
    /// Pairing the <see cref="JsonSerializerOptions"/> object where this option is overridden by the ones where
    /// it is not may breake round trip serialization / deserialization for same rangs of actual depths of 
    /// serialized objects. The user needs to ensure thtat the same object is used for serialization and 
    /// deserialization, and when this is not practical, depths are compatible or at least this does not
    /// affect conditions under which the serializer options are ued.</para></summary>
    public class JsonSerializerOptionsFactoryBasic
    {

        private static readonly object _staticLock = new object();

        private static volatile JsonSerializerOptions _default = null;

        /// <summary>Gets a singleton, lazy evaluated <see cref="JsonSerializerOptions"/>, read-only and
        /// thread safe object standard for use with the library in cases where polymorfic serialization / 
        /// deserialization is NOT required.
        /// <para>Used in factory methods, either directly returned or as basis for methods that enable
        /// altering some of the options, but can also be used dirctly.</para>
        /// <para>This object can be manually used as parameter of constructor <see cref="JsonSerializerOptions.JsonSerializerOptions(JsonSerializerOptions)"/>,
        /// such that only a limited number options are explicitly altered. For such use cases, try to use the safer
        /// factory methods of this class, which enable that a safer approach by offering only those options for
        /// alternation that don't break round trip serialization / deserialization.</para></summary>
        public static JsonSerializerOptions DefaultOptions {
            get
            {
                if (_default == null)
                {
                    lock (_staticLock)
                    {
                        if (_default == null)
                        {
                            var options = new JsonSerializerOptions()
                            {
                                ReferenceHandler = ReferenceHandler.Preserve,  // enables handling cyclic references and other
                                    // multiple references by using references instead of creating object copies (which would
                                    // also lead to infinite recursion in case of cyclic references)
                                ReadCommentHandling = JsonCommentHandling.Skip,  // allow // and /*  */ in JSON when deserialized
                                // Options below just emphasize the defaults to be used:
                                WriteIndented = false,  // by default, don't use indentation!
                            };
                            options.MakeReadOnly();
                            _default = options;
                        }
                    }
                }
                return _default;
            }
        }

        // Defaults:

        /// <summary>Library-wide default JSON serializer option for indented serialization strings.</summary>
        public const bool LibDefaultWriteIndented = false;

        /// <summary>Library-wide default JSON serializer option for indentation character in serialized
        /// strings, when indented. Set to space character.</summary>
        public const char LibDefaultIndentCharacter = ' ';

        /// <summary>Library-wide default JSON serializer option for number of indentation character, when
        /// serialization produces indented (more human readable) strings.</summary>
        public const int LibDefaultIndentSize = 2;

        /// <summary>Library-wide default JSON serializer option for maximum depth of object graph when
        /// serializing or dwserializing. Set to 64, which is .NET default.</summary>
        public const int LibDefaultMaxDepth = 64;


        /// <summary>Returns the static <see cref="JsonSerializerOptions"/> object that is locked for modification,
        /// obtained via the <see cref="DefaultOptions"/> static property.
        /// <para>This is a singleton object allocated once, therefore this method is very efficient.</para></summary>
        public JsonSerializerOptions Create()
        {
            return DefaultOptions;
        }

        /// <summary>Creeates and returns a <see cref="JsonSerializerOptions"/> object with this library's 
        /// standard connfiguration for NON-polymorfic JSON serialization / deserialization, where only the
        /// <see cref="JsonSerializerOptions.WriteIndented"/> option can be xhanged.</summary>
        /// <param name="writeIndented">Specifies whether JSON serialization produces a more human-readable
        /// string with multiple lines and indentation used to make understanding the structure of the 
        /// serialized string easier. Default is <see cref="LibDefaultWriteIndented"/></param>
        public JsonSerializerOptions Create(
            bool writeIndented = LibDefaultWriteIndented)
        {
            var options = new JsonSerializerOptions(DefaultOptions)
            {
                WriteIndented = writeIndented,
            };
            options.MakeReadOnly();
            return options;
        }

        /// <summary>Creeates and returns a <see cref="JsonSerializerOptions"/> object with this library's 
        /// standard connfiguration for NON-polymorfic JSON serialization / deserialization, where some options
        /// can be altered by the caller.
        /// <para>Most of the options that can be altered will not affect the round trip serialization / deserialization.
        /// Exceptionn is <paramref name="maxDepth"/>, which should therefore be used carefully and set (modified)
        /// only when this is really needed.</para>
        /// <para>All parameters are optional and take default values (the same values as in <see cref="DefaultOptions"/>)
        /// when not specified.</para></summary>
        /// <param name="writeIndented">Whether serialized strings should be more human-readable with newlines and
        /// indentation used according to object structure.</param>
        /// <param name="indentCharacter">Character used for indentation.</param>
        /// <param name="indentSize">Number of characters used to increase indentation by a single level.</param>
        /// <param name="maxDepth">Maximum depth of object graphs permitted by serialization / deserialization.</param>
        public JsonSerializerOptions Create(
            bool writeIndented = LibDefaultWriteIndented,
            char indentCharacter = LibDefaultIndentCharacter,
            int indentSize = LibDefaultIndentSize,
            int maxDepth = LibDefaultMaxDepth /* maps to default, which is 64 */
            )
        {
            var options = new JsonSerializerOptions(DefaultOptions)
            {
                WriteIndented = writeIndented,
                IndentCharacter = indentCharacter,
                IndentSize = indentSize,
                MaxDepth = maxDepth,
            };
            options.MakeReadOnly();
            return options;
        }


        /// <summary>Uses the current factory to generate a couple of JSON serialized options, and outputs
        /// the resulting objects to console.</summary>
        public static void Examples()
        {
            var factory = new JsonSerializerOptionsFactoryBasic();
            var optionsDefault = factory.Create();
            Console.WriteLine($"optionsDefault: \n{optionsDefault.ToJsonString(true)}\n");
            var optionsIndented = factory.Create(true);
            Console.WriteLine($"optionsIndented: \n{optionsIndented.ToJsonString(true)}\n");
            var optionsIndented1 = factory.Create(writeIndented: true);
            Console.WriteLine($"optionsIndented1: \n{optionsIndented1.ToJsonString(true)}\n");
            var optionsSpecific = factory.Create(writeIndented: true, maxDepth: LibDefaultMaxDepth * 2);
            Console.WriteLine($"optionsSpecific: \n{optionsSpecific.ToJsonString(true)}\n");
        }



    }

}