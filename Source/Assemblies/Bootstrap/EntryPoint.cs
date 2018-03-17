/*---------------------------------------------------------------------------------------------
 *  Copyright (c) Dolittle. All rights reserved.
 *  Licensed under the MIT License. See LICENSE in the project root for license information.
 *--------------------------------------------------------------------------------------------*/
using System.Reflection;
using Dolittle.Assemblies.Configuration;
using Dolittle.Assemblies.Rules;
using Dolittle.Logging;

namespace Dolittle.Assemblies.Bootstrap
{
    /// <summary>
    /// Represents the entrypoint for initializing assemblies
    /// </summary>
    public class EntryPoint
    {
        /// <summary>
        /// Initialize assemblies setup
        /// </summary>
        /// <param name="logger"><see cref="ILogger"/> to use for logging</param>
        /// <returns><see cref="IAssemblies"/></returns>
        public static IAssemblies Initialize(ILogger logger)
        {
            var assembliesConfigurationBuilder = new AssembliesConfigurationBuilder();
            assembliesConfigurationBuilder
                .ExcludeAll()
                .ExceptProjectLibraries()
                .ExceptDolittleLibraries();

            var assemblySpecifiers = new AssemblySpecifiers(assembliesConfigurationBuilder.RuleBuilder, logger);
            assemblySpecifiers.SpecifyUsingSpecifiersFrom(Assembly.GetEntryAssembly());

            var assembliesConfiguration = new AssembliesConfiguration(assembliesConfigurationBuilder.RuleBuilder);
            var assemblyFilters = new AssemblyFilters(assembliesConfiguration);

            var assemblyProvider = new AssemblyProvider(
                new ICanProvideAssemblies[] { new DefaultAssemblyProvider(logger)},
                assemblyFilters,
                new AssemblyUtility(),
                assemblySpecifiers);

            var assemblies = new Assemblies(assemblyProvider);
            return assemblies;
        }
    }

}