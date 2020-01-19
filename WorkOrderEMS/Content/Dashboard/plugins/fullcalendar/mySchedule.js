var eventObject = [];
var autocomplete;
let ManagerList;
var sourceFullView = { url: '/NewAdmin/GetMyEvents/' };
var sourceSummaryView = { url: '/NewAdmin/GetMyEvents/' };
var CalLoading = true;
var selectedManagers = "";
var arr=[];
function loadCalendar(ApplicantId) {
    $('#calendar').fullCalendar('destroy');
    CalLoading = true;
    $('#calendar').fullCalendar({
        header: {
            left: 'prev,next today',
            center: 'title',
            right: 'month,agendaWeek,agendaDay'
        },
        defaultView: 'month',
        editable: true,
        allDaySlot: false,
        selectable: true,
        slotMinutes: 60,
        events: '/NewAdmin/GetMyEvents/',

        eventClick: function (calEvent, jsEvent, view) {
            alert('You clicked on event id: ' + calEvent.id
                + "\nSpecial ID: " + calEvent.someKey
                + "\nAnd the title is: " + calEvent.title);

        },

        eventDrop: function (event, dayDelta, minuteDelta, allDay, revertFunc) {
            if (confirm("Confirm move?")) {
                UpdateEvent(event.id, event.start);
            }
            else {
                revertFunc();
            }
        },

        eventResize: function (event, dayDelta, minuteDelta, revertFunc) {

            if (confirm("Confirm change appointment length?")) {
                UpdateEvent(event.id, event.start, event.end);
            }
            else {
                revertFunc();
            }
        },



        dayClick: function (date, allDay, jsEvent, view) {
            debugger;
            getslots(date.format("DD-MMM-YYYY"));
            $('#eventTitle').val("");
            //$('#eventDate').val($.fullCalendar.formatDate(date, 'dd/MM/yyyy'));
            //$('#eventTime').val($.fullCalendar.formatDate(date, 'HH:mm'));
            $('#eventDate').val(date.format("MM/DD/YYYY"));
           // $('#eventTime').val(moment(date).format('HH:MM'));

            ShowEventPopup(date);
        },

        viewRender: function (view, element) {

            if (!CalLoading) {
                if (view.name == 'month') {
                    $('#calendar').fullCalendar('removeEventSource', sourceFullView);
                    $('#calendar').fullCalendar('removeEvents');
                    $('#calendar').fullCalendar('addEventSource', sourceSummaryView);
                }
                else {
                    $('#calendar').fullCalendar('removeEventSource', sourceSummaryView);
                    $('#calendar').fullCalendar('removeEvents');
                    $('#calendar').fullCalendar('addEventSource', sourceFullView);
                }
            }
        }

    });

    CalLoading = false;
    GetBookedSlots(ApplicantId);
    $.ajax({
        type: 'GET',
        url: '/NewAdmin/GetManagerList/',
        success: function (response) {
            if (response.length > 0) {
                ManagerList = response;
                autocomplete = new SelectPure(".autocomplete-select", {
                    options: ManagerList,
                    multiple: true,
                    autocomplete: true,
                    icon: "fa fa-times",
                    max: 3,
                    onChange: value => {

                        selectedManagers = autocomplete.value().toString();
                       
                    },
                    
                });

            }

        }
    });

}
function GetBookedSlots(ApplicantId) {
    $.ajax({
        type: 'GET',
        url: '/NewAdmin/GetBookedSlots/',
        data: { "ApplicantId": ApplicantId },
        success: function (response) {
            if (response.length > 0) {
                $("#BookedSlots")
                var $html = '<div id="UrgentWorkOrdersList"><ul class="list-group">';
                $.each(response, function (index, value) {
                    //$html += '<li class="list-group-item list-group-item-success"><label>Title: ' + value.title + '</label></br><label>' + value.startDate + '</label ></br >' + '<label>Start: ' + value.startTime + '</label></br>' + '<label>End: ' + value.end + '</label></br>';
                    $html += '<div class="row notice info">';
                    $html += '<div class="col-sm-12">';
                    $html += ' <p>Title: ' + value.title + '</p>';
                    $html += '</div>';
                    $html += '<div class="col-sm-4">';
                    $html += value.startDate + '</div>';
                    $html += '<div class="col-sm-8">Start: ';
                    $html += value.startTime + ' End: ' + value.end;
                    $html += '</div>';
                    $html += '</div>';
                });
                $html += '</ul></div>';
                $("#BookedSlots").html($html);

            }
            else {
            }
        }
    });
}

$('#btnPopupCancel').click(function () {
    ClearPopupFormValues();
    //$('#popupEventForm').hide();
    $("#ModalScheduleInterview").modal('hide');
});

$('#btnPopupSave').click(function () {

    //$('#popupEventForm').hide();
    $("#ModalScheduleInterview").modal('hide');
    var dataRow = {
        'Title': $('#eventTitle').val(),
        'NewEventDate': $('#eventDate').val(),
        //'NewEventTime': $('#eventTime').val(),
        'NewEventTime': $("#eventTime select option:selected").attr('id'),
        'NewEventDuration': $('#eventDuration').val(),
        'JobId': $("#JobId").val(),
        //'ApplicantName': $("#lblApplicantName").text(),
        //'ApplicantEmail': $("#lblApplicantEmail").text(),
        'selectedManagers': selectedManagers
    }

    ClearPopupFormValues();

    $.ajax({
        type: 'POST',
        url: "/NewAdmin/SaveEvent",
        data: dataRow,
        success: function (response) {
            $('#calendar').fullCalendar('refetchEvents');
            alert(response);
            GetBookedSlots($("#lblApplicantId").text());

        }
    });
});

function ShowEventPopup(date) {
    ClearPopupFormValues();
    //$('#popupEventForm').show();
    $("#ModalScheduleInterview").modal('show');
    $('#eventTitle').focus();
}

function ClearPopupFormValues() {
    $('#eventID').val("");
    $('#eventTitle').val("");
    $('#eventDateTime').val("");
    $('#eventDuration').val("");
}

function UpdateEvent(EventID, EventStart, EventEnd) {

    var dataRow = {
        'ID': EventID,
        'NewEventStart': EventStart,
        'NewEventEnd': EventEnd
    }

    //$.ajax({
    //    type: 'POST',
    //    url: "/NewAdmin/UpdateEvent",
    //    dataType: "json",
    //    contentType: "application/json",
    //    data: JSON.stringify(dataRow)
    //});
}
var SelectPure = function () { "use strict"; const e = { value: "data-value", disabled: "data-disabled", class: "class", type: "type" }; class t { constructor(e, t = {}, s = {}) { return this._node = e instanceof HTMLElement ? e : document.createElement(e), this._config = { i18n: s }, this._setAttributes(t), t.textContent && this._setTextContent(t.textContent), this } get() { return this._node } append(e) { return this._node.appendChild(e), this } addClass(e) { return this._node.classList.add(e), this } removeClass(e) { return this._node.classList.remove(e), this } toggleClass(e) { return this._node.classList.toggle(e), this } addEventListener(e, t) { return this._node.addEventListener(e, t), this } removeEventListener(e, t) { return this._node.removeEventListener(e, t), this } setText(e) { return this._setTextContent(e), this } getHeight() { return window.getComputedStyle(this._node).height } setTop(e) { return this._node.style.top = `${e}px`, this } focus() { return this._node.focus(), this } _setTextContent(e) { this._node.textContent = e } _setAttributes(t) { for (const s in t) e[s] && t[s] && this._setAttribute(e[s], t[s]) } _setAttribute(e, t) { this._node.setAttribute(e, t) } } var s = Object.assign || function (e) { for (var t = 1; t < arguments.length; t++) { var s = arguments[t]; for (var i in s) Object.prototype.hasOwnProperty.call(s, i) && (e[i] = s[i]) } return e }; const i = { select: "select-pure__select", dropdownShown: "select-pure__select--opened", multiselect: "select-pure__select--multiple", label: "select-pure__label", placeholder: "select-pure__placeholder", dropdown: "select-pure__options", option: "select-pure__option", autocompleteInput: "select-pure__autocomplete", selectedLabel: "select-pure__selected-label", selectedOption: "select-pure__option--selected", placeholderHidden: "select-pure__placeholder--hidden", optionHidden: "select-pure__option--hidden" }; return class { constructor(e, i) { this._config = s({}, i), this._state = { opened: !1 }, this._icons = [], this._boundHandleClick = this._handleClick.bind(this), this._boundUnselectOption = this._unselectOption.bind(this), this._boundSortOptions = this._sortOptions.bind(this), this._body = new t(document.body), this._create(e), this._config.value && this._setValue() } value() { return this._config.value } _create(e) { const s = "string" == typeof e ? document.querySelector(e) : e; this._parent = new t(s), this._select = new t("div", { class: i.select }), this._label = new t("span", { class: i.label }), this._optionsWrapper = new t("div", { class: i.dropdown }), this._config.multiple && this._select.addClass(i.multiselect), this._options = this._generateOptions(), this._select.addEventListener("click", this._boundHandleClick), this._select.append(this._label.get()), this._select.append(this._optionsWrapper.get()), this._parent.append(this._select.get()), this._placeholder = new t("span", { class: i.placeholder, textContent: this._config.placeholder }), this._select.append(this._placeholder.get()) } _generateOptions() { return this._config.autocomplete && (this._autocomplete = new t("input", { class: i.autocompleteInput, type: "text" }), this._autocomplete.addEventListener("input", this._boundSortOptions), this._optionsWrapper.append(this._autocomplete.get())), this._config.options.map(e => { const s = new t("div", { class: i.option, value: e.value, textContent: e.label, disabled: e.disabled }); return this._optionsWrapper.append(s.get()), s }) } _handleClick(e) { if (e.stopPropagation(), e.target.className !== i.autocompleteInput) { if (this._state.opened) { const t = this._options.find(t => t.get() === e.target); return t && this._setValue(t.get().getAttribute("data-value"), !0), this._select.removeClass(i.dropdownShown), this._body.removeEventListener("click", this._boundHandleClick), this._select.addEventListener("click", this._boundHandleClick), void (this._state.opened = !1) } e.target.className !== this._config.icon && (this._select.addClass(i.dropdownShown), this._body.addEventListener("click", this._boundHandleClick), this._select.removeEventListener("click", this._boundHandleClick), this._state.opened = !0, this._autocomplete && this._autocomplete.focus()) } } _setValue(e, t, s) { if (e && !s && (this._config.value = this._config.multiple ? [...this._config.value || [], e] : e), e && s && (this._config.value = e), this._options.forEach(e => { e.removeClass(i.selectedOption) }), this._placeholder.removeClass(i.placeholderHidden), this._config.multiple) { const e = this._config.value.map(e => { const t = this._config.options.find(t => t.value === e); return this._options.find(e => e.get().getAttribute("data-value") === t.value.toString()).addClass(i.selectedOption), t }); return e.length && this._placeholder.addClass(i.placeholderHidden), void this._selectOptions(e, t) } const n = this._config.value ? this._config.options.find(e => e.value.toString() === this._config.value) : this._config.options[0]; this._options.find(e => e.get().getAttribute("data-value") === n.value.toString()).addClass(i.selectedOption), this._placeholder.addClass(i.placeholderHidden), this._selectOption(n, t) } _selectOption(e, t) { this._selectedOption = e, this._label.setText(e.label), this._config.onChange && t && this._config.onChange(e.value) } _selectOptions(e, s) { this._label.setText(""), this._icons = e.map(e => { const s = new t("span", { class: i.selectedLabel, textContent: e.label }), n = new t(this._config.inlineIcon ? this._config.inlineIcon.cloneNode(!0) : "i", { class: this._config.icon, value: e.value }); return n.addEventListener("click", this._boundUnselectOption), s.append(n.get()), this._label.append(s.get()), n.get() }), s && this._optionsWrapper.setTop(Number(this._select.getHeight().split("px")[0]) + 5), this._config.onChange && s && this._config.onChange(this._config.value) } _unselectOption(e) { const t = [...this._config.value], s = t.indexOf(e.target.getAttribute("data-value")); -1 !== s && t.splice(s, 1), this._setValue(t, !0, !0) } _sortOptions(e) { this._options.forEach(t => { t.get().textContent.toLowerCase().startsWith(e.target.value.toLowerCase()) ? t.removeClass(i.optionHidden) : t.addClass(i.optionHidden) }) } } }();

function ShowMyOpeningGrid() {
    selectedManagers = "";
    $('#calendar').fullCalendar('destroy');
    CalLoading = true;
    //$("#JobPostBackBtn").show();
    $("#Scheduler").css('display', 'none');
    $("#MyOpeningSummery").show();
    $("#inpManageName").html('');
    $("#inpManageName").html('<span class="autocomplete-select"></span>');
    $("#lblApplicantId").text('');
    $("#lblApplicantName").text('');
    $("#lblApplicantEmail").text('');
}

