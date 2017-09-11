if (window.parent) {
    window.parent.iframeLoaded();
}


//overriddes 
var overrideFuns = {
    addGridLiveSearchFunctionality: function (el) {
        setTimeout(function () {
            var table = el.find('.dx-datagrid-headers table');
            table.append('<tr class="filter-search"><td colspan="10"><input type="text" placeholder="Filter (Start typing)"></td></tr>');
            var filterInput = table.find('input[type=text]');
            filterInput.on('input', function () {
                search(this.value);
            });
            el.find('.dx-datagrid-action').on('click', function () {
                setTimeout(function () {
                    search(filterInput[0].value);
                }, 50);
            });

            function search(text) {
                var trs = el.find('.dx-datagrid-rowsview table .dx-data-row');
                trs.each(function (i, tr) {
                    var notAMatch = true;
                    tr.childNodes.forEach(function (td) {
                        if (td.textContent.toLowerCase().indexOf(text.toLowerCase()) > -1) {
                            notAMatch = false;
                        }
                    });
                    $(tr)[notAMatch && 'addClass' || 'removeClass']('filter-hide');
                });
                //$('dx-datagrid-rowsview').trigger('scroll');
                setTimeout(function () {
                    console.log("TRIGGER");
                    $('.dx-datagrid-rowsview').trigger('scroll');
                    $('.dx-scrollable-content').css('transform', 'translate(0px, 0px)');
                    //$('.dx-datagrid-rowsview').animate({ scrollTop: 200 }, 600);
                }, 100);
            }
        }, 100);
    }
};

$(function () {
    setTimeout(function () {
        $(".loading").removeClass("loading");
    }, 4000);
    $('body').addClass('filter-float');
    if (/xID=.*mobile&?.*$/i.test(window.location.href)) {
        $('body').addClass('mobile-layout');
    } else {
        $('body').addClass('desktop-layout');
    }
    $(document).on('click', '#filterBtn,.filter-backdrop', function () {
        $('body').toggleClass('show-filter');
    });
    window.toggleFilters = function () {
        $('body').toggleClass('show-filter');
    };
    //dashboard event listeners
    var firstWidgetLoaded = false;
    $(document.body).on('dashboardViewerWidgetCreated', function (e, data) {
        var widget = data.GetWidget();
        if (widget.NAME === 'dxDataGrid') {

            if (data.ItemName == "gridDashboardItem1") {
                // supress zero values
                widget.option('dataSource.filter', ["DataItem2", "<>", 0]);
            }

            overrideFuns.addGridLiveSearchFunctionality(widget._$element);
        }
        if (!firstWidgetLoaded) {
            firstWidgetLoaded = true;
            parentWindowEvent('iframe-first-widget-loaded');
        }
    });
    $(document.body).on('dashboardViewerWidgetUpdated', function (e, data) {
        var widget = data.GetWidget();
        if (widget.NAME === 'dxDataGrid') {
            overrideFuns.addGridLiveSearchFunctionality(widget._$element);
        }
    });
});

var runWhenLoaded = [
    //Ann Monitor dashboard - remove widget default card shadow from top counts widget
    function () {
        var el = $('.filter-float #clearBtn');
        if (el.length) {
            el.click(function () {
                window.location.reload();
            });
            return true;
        }
    },
    function () {
        var el = $('[data-layout-item-name="textBoxDashboardItem1"]');
        var el2 = $('[data-layout-item-name="textBoxDashboardItem2"]');
        if (el.length) {
            el.parent().removeClass('dx-dashboard-splitter-pane');
            el2.parent().removeClass('dx-dashboard-splitter-pane');
            //return true;
        }
    },
    function () {
        var el = $('[data-layout-item-name="dashboardItemGroup2"],[data-layout-item-name="dashboardItemGroup1"]');
        if (el.length) {
            el.addClass('filter-pane');
            $('<div class="filter-backdrop"></div>').insertBefore(el);
            setTimeout(function () {
                $('body').addClass('filters-loaded');
            }, 2000);
            return true;
        }
    }
];

var loadedFunInterval = setInterval(function () {
    var itemsToRemove = [];
    for (var i = 0; i < runWhenLoaded.length; i++) {
        var fun = runWhenLoaded[i];
        if (fun()) {
            itemsToRemove.push(i);
        }
    }
    for (var i = itemsToRemove.length - 1; i >= 0; i--) {
        runWhenLoaded.splice(itemsToRemove[i], 1);
    }
    if (!runWhenLoaded.length) {
        clearInterval(loadedFunInterval);
    }
}, 40);

function parentWindowEvent(event, data) {
    window.parent.$rootScope.$emit(event, data);
}

function dashboardViewerLoaded() {
    parentWindowEvent('iframe-dashboard-loaded');
}

function dashboardEndCallback() {
    parentWindowEvent('iframe-dashboard-end-callback');
    var el = $('.dx-dashboard-container .dx-dashboard-title .dx-dashboard-title-filter-image');
    if (el.length) {
        el.trigger('mouseenter');
        setTimeout(function () {
            parentWindowEvent('iframe-dashboard-title-tooltip', $('.dx-dashboard-container .dx-overlay-wrapper .dx-dashboard-title-tooltip').html());
            setTimeout(function () {
                el.trigger('mouseleave');
            });
        });
    }
}
