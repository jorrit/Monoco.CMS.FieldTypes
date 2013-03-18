// ------------------------------------------------------------------
// Linklist
// Developed by Andreas Bergström, Monoco [andreas@monoco.se]
// This script file is needed in order for the link list field type
// to work. The field type will not work without it and it should not
// be edited. 
// Script is registered by the Monoco.CMS.Pipelines.RenderContentEditor.AddLinkListScripts pipeline.
// Pipeline is registered from the Monoco.CMS.FieldType.config include file.
// ------------------------------------------------------------------
scContentEditor.prototype.linklistDblClick = function(id) {
    var source = scForm.browser.getControl(id + '_List');
    var index = source.selectedIndex;
    if (index == -1)
        return;
    scForm.postRequest('', '', '', 'action:edit(id=' + id + ', index=' + index + ')');
};
scContentEditor.prototype.linklistMoveUp = function (id) {

    var source = scForm.browser.getControl(id + '_List');

    var options = source.getElementsByTagName('option');

    for (var i = 1; i < options.length; i++) {
        var option = options[i];
        if (option.selected) {
            source.removeChild(option);
            source.insertBefore(option, options[i - 1]);

            scForm.postRequest('', '', '', 'action:move(id=' + id + ',direction=up,index=' + i + ')');
        }
    }
};
scContentEditor.prototype.linklistMoveDown = function (id) {

    var source = scForm.browser.getControl(id + '_List');

    var options = source.getElementsByTagName('option');

    for (var i = options.length - 2; i >= 0; i--) {
        var option = options[i];
        if (option.selected) {

            var nextOption = options[i + 1];
            var option = source.removeChild(option);
            var nextOption = source.replaceChild(option, nextOption);
            source.insertBefore(nextOption, option);

            scForm.postRequest('', '', '', 'action:move(id=' + id + ',direction=down,index=' + i + ')');
        }
    }
};
scContentEditor.prototype.updateLink = function (id, params) {
    var c = scForm.browser.getControl(id + '_List');
    c.options[params.index].text = params.text;
};

scContentEditor.prototype.linklistUpdateLink = function (id, params) {
    var c = scForm.browser.getControl(id + '_List');
    c.options[params.index].text = params.text;
};
scContentEditor.prototype.linklistInsertLink = function (id, params) {
    var c = scForm.browser.getControl(id + '_List');
    var n = document.createElement('option');
    n.text = params.text;
    c.options.add(n);
};
scContentEditor.prototype.linklistMove = function (id, params) {
    var d = params.direction;
    var i = this.linklistGetSelectedIndex(id);
    scForm.postRequest('', '', '', 'action:move(id=' + id + ', direction=' + d + ', index=' + i + ')');
};
scContentEditor.prototype.linklistGetSelectedIndex = function (id) {
    var c = scForm.browser.getControl(id + '_List');
    for (var i = 0; i < c.options.length; i++) {
        if (c.options.options[i].selected) {
            return i;
        }
    }
    return -1;
};
scContentEditor.prototype.linklistEdit = function (id) {
    var c = scForm.browser.getControl(id + '_List');

    var index = c.selectedIndex;
    if (index == -1)
        return;
    
    scForm.postRequest('', '', '', 'action:edit(id=' + id + ', index=' + index + ')');

};

scContentEditor.prototype.linklistRemoveLink = function (id, params) {
    var control = scForm.browser.getControl(id + '_List');
    control.remove(params.index);
};
scContentEditor.prototype.linklistDelete = function (id) {
    var c = scForm.browser.getControl(id + '_List');

    var index = c.selectedIndex;
    
    if (c.selectedIndex == -1)
        return;

    scForm.postRequest('', '', '', 'action:delete(id=' + id + ', remove=' + index + ')');
};

scContentEditor.prototype.linklistUpdate = function (id) {

    var selected = scForm.browser.getControl(id + "_List");

    // IE listbox hack
    selected.style.position = selected.style.position == 'relative' ? 'static' : 'relative';

    var value = "";

    for (var n = 0; n < selected.options.length; n++) {
        var option = selected.options[n];
        value += (value != "" ? "|" : "") + option.value;
    }

    var selectedValues = scForm.browser.getControl(id + "_Value");

    selectedValues.value = value;
};

// ------------------------------------------------------------------
// Linklist (end.)
// ------------------------------------------------------------------
