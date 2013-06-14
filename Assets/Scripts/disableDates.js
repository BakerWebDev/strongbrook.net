﻿function getElementsByClassName(a, b) {
    var c = [];
    var d = a.getElementsByTagName("*");
    for (var e = 0; e < d.length; e++) {
        if (d[e].className.indexOf(b) != -1) {
            c.push(d[e])
        }
    }
    return c
}

function DisableDates(a) {
    if (a._element.className.indexOf("disable_past_dates") != -1) {
        var b = getElementsByClassName(a._days, "ajax__calendar_day");
        var c = (new Date).setHours(0, 0, 0, 0);
        for (var d = 0; d < b.length; d++) {
            try {
                if (b[d].date.setHours(0, 0, 0, 0) >= c) {
                    b[d].className = "ajax__calendar_day"
                } else {
                    b[d].className = "ajax__calendar_day ajax__calendar_day_disabled"
                }
            } catch (e) { }
        }
    }
    if (a._element.className.indexOf("disable_future_dates") != -1) {
        var b = getElementsByClassName(a._days, "ajax__calendar_day");
        var c = (new Date).setHours(0, 0, 0, 0);
        for (var d = 0; d < b.length; d++) {
            try {
                if (b[d].date.setHours(0, 0, 0, 0) <= c) {
                    b[d].className = "ajax__calendar_day"
                } else {
                    b[d].className = "ajax__calendar_day ajax__calendar_day_disabled"
                }
            } catch (e) { }
        }
    }
}
AjaxControlToolkit.CalendarBehavior.prototype.show = function (a) {
    this._ensureCalendar();
    if (!this._isOpen) {
        var b = new Sys.CancelEventArgs;
        this.raiseShowing(b);
        if (b.get_cancel()) {
            return
        }
        this._isOpen = true;
        this._popupBehavior.show();
        if (this._firstPopUp) {
            this._switchMonth(null, true);
            switch (this._defaultView) {
                case AjaxControlToolkit.CalendarDefaultView.Months:
                    this._switchMode("months", true);
                    break;
                case AjaxControlToolkit.CalendarDefaultView.Years:
                    this._switchMode("years", true);
                    break
            }
            this._firstPopUp = false
        }
        this.raiseShown();
        DisableDates(this)
    }
};
AjaxControlToolkit.CalendarBehavior.prototype._cell_onclick = function (a) {
    a.stopPropagation();
    a.preventDefault();
    if (!this._enabled) return;
    var b = a.target;
    var c = this._getEffectiveVisibleDate();
    Sys.UI.DomElement.removeCssClass(b.parentNode, "ajax__calendar_hover");
    switch (b.mode) {
        case "prev":
        case "next":
            this._switchMonth(b.date);
            break;
        case "title":
            switch (this._mode) {
                case "days":
                    this._switchMode("months");
                    break;
                case "months":
                    this._switchMode("years");
                    break
            }
            break;
        case "month":
            if (b.month == c.getMonth()) {
                this._switchMode("days")
            } else {
                this._visibleDate = b.date;
                this._switchMode("days")
            }
            break;
        case "year":
            if (b.date.getFullYear() == c.getFullYear()) {
                this._switchMode("months")
            } else {
                this._visibleDate = b.date;
                this._switchMode("months")
            }
            break;
        case "day":
            if (this._element.className.indexOf("disable_past_dates") != -1) {
                if (b.date.setHours(0, 0, 0, 0) >= (new Date).setHours(0, 0, 0, 0)) {
                    this.set_selectedDate(b.date);
                    this._switchMonth(b.date);
                    this._blur.post(true);
                    this.raiseDateSelectionChanged()
                }
            } else if (this._element.className.indexOf("disable_future_dates") != -1) {
                if (b.date.setHours(0, 0, 0, 0) <= (new Date).setHours(0, 0, 0, 0)) {
                    this.set_selectedDate(b.date);
                    this._switchMonth(b.date);
                    this._blur.post(true);
                    this.raiseDateSelectionChanged()
                }
            } else {
                this.set_selectedDate(b.date);
                this._switchMonth(b.date);
                this._blur.post(true);
                this.raiseDateSelectionChanged()
            }
            break;
        case "today":
            this.set_selectedDate(b.date);
            this._switchMonth(b.date);
            this._blur.post(true);
            this.raiseDateSelectionChanged();
            break
    }
    DisableDates(this)
};