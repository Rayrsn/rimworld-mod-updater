// This code is injected into the browser page is responsible for overridng the behaviour of the steam subscribe buttons to instead download the mod.
// Obviously only works in the updater's embedded browser.

// Could probably make an alternate userscript/extension version that uses a URI protocol/websocket to communicate with the updater for easy downloading but I've put enough work into this for one night...

// You can put your own stuff in here to modify the page if you want. The only things exposed from the updater to the browser are 
// _____updater.doDownload
// _____updater.doDownloadId
// These only call an event in .NET, which isn't exposed to the browser.

if (window.location.host == "steamcommunity.com") {
    window.SubscribeItem = function (id, appid) {
        if (id != null) {
            window._____updater.doDownloadId(id)
            return
        }
        window._____updater.doDownload()
    }

    window.SubscribeCollection = function () {
        window._____updater.doDownload()
    }

    window.SubscribeCollectionItem = function (id, appid) {
        if (id != null) {
            window._____updater.doDownloadId(id)
        }
    }

    // Get the correct aspect ratio for download buttons on item cards
    function extractAspectRatioClass() {
        let ret = "aspectratio_16x9" // default value

        let preview = document.getElementsByClassName("workshopItemPreviewImage")[0]
        if (preview) {
            let classes = preview.getAttribute("class").split(" ")
            classes.forEach((aclass) => {
                if (aclass.startsWith("aspectratio")) {
                    return aclass
                }
            })
        }

        return ret
    }

    if (document.URL.includes("browse")) // 10/10 url matching
    {
        // This section is responsible for creating the little mini subscribe buttons that sit in the bottom right of workshop item cards when browsing for mods.
        // Normally these are only created when the user is logged in, hence why we're creating them manually.
        // This will do nothing if the user is logged in, but the download override will still work because the default buttons use SubscribeItem()
        let items = document.getElementsByClassName("workshopItem")

        // get proper aspect ratio(probably)
        let aspectRatio = extractAspectRatioClass()

        Array.prototype.forEach.call(items, (item) => {
            let ugc = item.getElementsByClassName("ugc")[0]
            if (!ugc) return;
            let id = ugc.getAttribute("data-publishedfileid") // Get item id
            if (!id) return;

            let controls = item.getElementsByClassName("workshopItemSubscriptionControls")[0]
            if (controls == null) { // Don't create download button if there's already one there.

                let controls = document.createElement("div")
                controls.setAttribute("class", "workshopItemSubscriptionControls " + aspectRatio);

                item.insertBefore(controls, ugc.nextSibling)

                let btn = document.createElement("a")
                btn.setAttribute("class", "general_btn subscribe");
                controls.appendChild(btn)


                let div = document.createElement("div")
                div.setAttribute("class", "subscribeIcon")

                btn.appendChild(div)

                btn.on("click", () => {
                    window._____updater.doDownloadId(id)
                })
            }
        })
    }

    let div = document.getElementById("SubscribeItemOptionAdd")


    // Change the text on various things to say download.
    // super fancy.
    if (div) {
        div.innerHTML = "Download"
    } else {
        let btn = document.getElementById("SubscribeItemBtn")

        if (btn) {
            let div2 = btn.getElementsByClassName("subscribeText")[0]
            if (div2) {
                div2.innerHTML = "Download all items"
            }

            let col = document.getElementsByClassName("collectionChildren")[0]
            if (col) {
                let btn = col.getElementsByClassName("general_btn subscribe")[1]
                if (btn) {
                    btn.remove()
                }
            }
        }
    }

    
}
