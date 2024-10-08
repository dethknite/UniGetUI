// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

using Microsoft.Management.Deployment;

namespace WindowsPackageManager.Interop;

/// <summary>
/// Factory class for creating WinGet COM objects.
/// Details about each method can be found in the source IDL:
/// https://github.com/microsoft/winget-cli/blob/master/src/Microsoft.Management.Deployment/PackageManager.idl
/// </summary>
public abstract class WindowsPackageManagerFactory
{
    private readonly ClsidContext _clsidContext;
    protected readonly bool _allowLowerTrustRegistration;

    public WindowsPackageManagerFactory(ClsidContext clsidContext, bool allowLowerTrustRegistration = false)
    {
        _clsidContext = clsidContext;
        _allowLowerTrustRegistration = allowLowerTrustRegistration;
    }

    /// <summary>
    /// Creates an instance of the class <typeparamref name="T"/>.
    /// </summary>
    /// <remarks>
    /// Type <typeparamref name="T"/> must be one of the types defined in the winget COM API.
    /// Implementations of this method can assume that <paramref name="clsid"/> and <paramref name="iid"/>
    /// are the right GUIDs for the class in the given context.
    /// </remarks>
    protected abstract T CreateInstance<T>(Guid clsid, Guid iid);

    public PackageManager CreatePackageManager() => CreateInstance<PackageManager>();

    public FindPackagesOptions CreateFindPackagesOptions() => CreateInstance<FindPackagesOptions>();

    public CreateCompositePackageCatalogOptions CreateCreateCompositePackageCatalogOptions() => CreateInstance<CreateCompositePackageCatalogOptions>();

    public InstallOptions CreateInstallOptions() => CreateInstance<InstallOptions>();

    public UninstallOptions CreateUninstallOptions() => CreateInstance<UninstallOptions>();

    public PackageMatchFilter CreatePackageMatchFilter() => CreateInstance<PackageMatchFilter>();

    /// <summary>
    /// Creates an instance of the class <typeparamref name="T"/>.
    /// </summary>
    /// <remarks>
    /// This is a helper for calling the derived class's <see cref="CreateInstance{T}(Guid, Guid)"/>
    /// method with the appropriate GUIDs.
    /// </remarks>
    private T CreateInstance<T>()
    {
        Guid clsid = ClassesDefinition.GetClsid<T>(_clsidContext);
        Guid iid = ClassesDefinition.GetIid<T>();
        return CreateInstance<T>(clsid, iid);
    }
}
