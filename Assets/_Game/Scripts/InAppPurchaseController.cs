using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Purchasing;
using UnityEngine.Purchasing.Extension;
using UnityEngine.Purchasing.Security;

public class InAppPurchaseController : MonoBehaviour, IDetailedStoreListener
{
    private sealed class _RestorePurchases_c__AnonStorey0
    {
        internal UnityAction<bool> callback;

        internal InAppPurchaseController _this;

        internal void __m__0(bool result)
        {
            this._this.isRestoringPurchases = false;
            if (this.callback != null)
            {
                this.callback(result);
            }
        }
    }

    private static InAppPurchaseController _Instance_k__BackingField;

    private IStoreController m_StoreController;
    private IExtensionProvider m_StoreExtensionProvider;
    private IGooglePlayStoreExtensions m_GooglePlayStoreExtensions;
    private IAppleExtensions m_AppleExtensions;
    bool m_IsGooglePlayStoreSelected = false;

    private CrossPlatformValidator validator;

    private UnityAction<string> buyProductCallback;

    private UnityAction<string> buyProductCallbackDefault;

    private UnityAction initializedCallback;

    private bool isRestoringPurchases = false;

    public static InAppPurchaseController Instance
    {
        get;
        private set;
    }

    private void Awake()
    {
        if (InAppPurchaseController.Instance == null)
        {
            InAppPurchaseController.Instance = this;
        }
        else
        {
            UnityEngine.Object.Destroy(this);
        }
        UnityEngine.Object.DontDestroyOnLoad(this);
    }

    public void InitializePurchasing(ProductIAP[] listProduct, UnityAction<string> buyCallback, UnityAction initCallback = null)
    {
        if (this.IsInitialized())
        {
            if (initCallback != null)
            {
                initCallback();
            }
            return;
        }
        this.buyProductCallback = buyCallback;
        this.buyProductCallbackDefault = buyCallback;
        this.initializedCallback = initCallback;
        m_IsGooglePlayStoreSelected = Application.platform == RuntimePlatform.Android && StandardPurchasingModule.Instance().appStore == AppStore.GooglePlay;

        var builder = ConfigurationBuilder.Instance(StandardPurchasingModule.Instance());
        for (int i = 0; i < listProduct.Length; i++)
        {
            builder.AddProduct(listProduct[i].productId, listProduct[i].productType);
        }
        UnityPurchasing.Initialize(this, builder);
    }

    public void BuyProductID(string productId, UnityAction<string> callback = null)
    {
        Singleton<Popup>.Instance.ShowInstantLoading(15);
        if (this.IsInitialized())
        {
            Product product = this.m_StoreController.products.WithID(productId);

            if (product != null && product.availableToPurchase)
            {

                if (callback != null)
                {
                    UnityEngine.Debug.Log("IAP - call back is not null");
                    this.buyProductCallback = callback;
                }
                else
                {
                    UnityEngine.Debug.Log("IAP - call back is default");
                    this.buyProductCallback = this.buyProductCallbackDefault;
                }
                this.m_StoreController.InitiatePurchase(product);
            }
        }
    }

    public void RestorePurchases(System.Action<bool, string> onRestoreCompleted)
    {
        if (!this.IsInitialized())
        {
            return;
        }
        if (this.isRestoringPurchases)
            return;
        Debug.Log("[IAPManager] Request RestorePurchases");

        this.isRestoringPurchases = true;
        System.Action<bool, string> cb = (complete, callback) =>
        {
            this.isRestoringPurchases = false;
            onRestoreCompleted(complete, callback);
        };

        if (m_IsGooglePlayStoreSelected)
        {
            Debug.Log("[IAPManager] Restore transaction Google.");
            m_GooglePlayStoreExtensions.RestoreTransactions(cb);
        }
        else
        {
            Debug.Log("[IAPManager] Restore transaction Apple.");
            m_AppleExtensions.RestoreTransactions(cb);
        }
    }

    public Product GetProduct(string productId)
    {
        Product result = null;
        if (this.IsInitialized())
        {
            for (int i = 0; i < this.m_StoreController.products.all.Length; i++)
            {
                if (string.Equals(productId, this.m_StoreController.products.all[i].definition.id, System.StringComparison.Ordinal))
                {
                    result = this.m_StoreController.products.all[i];
                    break;
                }
            }
        }
        return result;
    }

    public Product[] GetAllProducts()
    {
        if (this.IsInitialized())
        {
            return this.m_StoreController.products.all;
        }
        return null;
    }

    public bool IsInitialized()
    {
        return this.m_StoreController != null && this.m_StoreExtensionProvider != null;
    }

    public void OnInitializeFailed(InitializationFailureReason error)
    {
        Debug.Log("[IAPManager] OnInitializeFailed InitializationFailureReason:" + error);
    }

    public void OnInitializeFailed(InitializationFailureReason error, string e)
    {
        Debug.Log("[IAPManager] OnInitializeFailed InitializationFailureReason:" + error);
    }

    public void OnInitialized(IStoreController controller, IExtensionProvider extensions)
    {
        this.m_StoreController = controller;
        this.m_StoreExtensionProvider = extensions;
        m_AppleExtensions = extensions.GetExtension<IAppleExtensions>();
        m_GooglePlayStoreExtensions = extensions.GetExtension<IGooglePlayStoreExtensions>();

        if (this.initializedCallback != null)
        {
            this.initializedCallback();
        }
    }

    public void OnPurchaseFailed(Product i, PurchaseFailureReason p)
    {
        Singleton<Popup>.Instance.HideInstantLoading();
        Debug.Log($"[IAPManager] OnPurchaseFailed, ProductID {i.definition.id} with reason = {p}");
        ShowPurchaseFailed(p);
    }

    public void OnPurchaseFailed(Product product, PurchaseFailureDescription failureDescription)
    {
        Singleton<Popup>.Instance.HideInstantLoading();
        Debug.Log($"[IAPManager] OnPurchaseFailed, ProductID {failureDescription.productId} with reason = {failureDescription.reason} and message = {failureDescription.message}");
        ShowPurchaseFailed(failureDescription.reason);
    }

    public PurchaseProcessingResult ProcessPurchase(PurchaseEventArgs args)
    {
        Singleton<Popup>.Instance.HideInstantLoading();
        var product = args.purchasedProduct;
        Debug.Log($"[IAPManager] ProcessPurchase, Buying {product.definition.id}");
        this.buyProductCallbackDefault?.Invoke(product.definition.id);
        return PurchaseProcessingResult.Complete;
    }

    void ShowPurchaseFailed(PurchaseFailureReason reason)
    {
        string textReason = "";
        switch (reason)
        {
            case PurchaseFailureReason.PurchasingUnavailable:
                textReason = "Purchasing Unavailable!";
                break;
            case PurchaseFailureReason.ExistingPurchasePending:
                textReason = "Purchase is pending!";
                break;
            case PurchaseFailureReason.ProductUnavailable:
                textReason = "Product Unavailable!";
                break;
            case PurchaseFailureReason.SignatureInvalid:
                textReason = "Signature Invaild!";
                break;
            case PurchaseFailureReason.UserCancelled:
                textReason = "User cancel!";
                break;
            case PurchaseFailureReason.PaymentDeclined:
                textReason = "Payment Declined!";
                break;
            case PurchaseFailureReason.DuplicateTransaction:
                textReason = "Duplicate transactio, let restore purchase!";
                break;
        }

        Singleton<Popup>.Instance.Show("Please retry later!\nReason: " + textReason, "PURCHASE FAILED!", PopupType.Ok);
    }
}
