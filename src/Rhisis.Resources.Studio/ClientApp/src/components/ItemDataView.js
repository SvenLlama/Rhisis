import React, { Component } from 'react';
import { DataTable } from 'primereact/datatable';
import { Column } from 'primereact/column';
import { Button } from 'primereact/button';
import { MultiSelect } from 'primereact/multiselect';
import { unique } from 'jquery';


export class ItemDataView extends Component {
    static displayName = ItemDataView.name;

    constructor(props) {
        super(props);
        this.state = { itemDatas: [], loading: true, selectedColumns: [] };

        this.setRef = this.setRef.bind(this);
        this.onColumnToggle = this.onColumnToggle.bind(this);      
        this.rawColumns = [
            { field: "id", sortable: true, filter: true, filterMatchMode: "contains" },
            { field: "name", sortable: true, filter: true, filterMatchMode: "contains" },
            { field: "level", sortable: true, filter: true },
            { field: "job", sortable: true, filter: true, multiSelect: true },
            { field: "kindOne", sortable: true, filter: true, multiSelect: true },
            { field: "kindTwo", sortable: true, filter: true, multiSelect: true  },
            { field: "kindThree", sortable: true, filter: true, multiSelect: true },
            { field: "parts", sortable: true, filter: true, multiSelect: true }
        ];
    }

    setRef(el) {
        this.dt = el;
    }

    componentDidMount() {
        this.populateItemData();
    }

    onColumnToggle(event) {
        let selectedColumns = event.value;
        let orderedSelectedColumns = this.columns.filter(col => selectedColumns.some(sCol => sCol.name === col.name));
        this.setState({ selectedColumns: orderedSelectedColumns });
    }

    render() {
        let itemDatas = this.state.itemDatas;

        const paginatorLeft = <Button type="button" icon="pi pi-refresh" className="p-button-text" />;
        const paginatorRight = <Button type="button" icon="pi pi-cloud" className="p-button-text" />;

        const header = (
            <div style={{ textAlign: 'left' }}>
                <MultiSelect value={this.state.selectedColumns} options={this.columns} optionLabel="name" onChange={this.onColumnToggle} style={{ width: '20em' }} />
            </div>
        );

        const columnComponents = this.state.selectedColumns.map(col => col.column());

        return (
            <div>
                <div className="card">
                    <DataTable
                        ref={this.setRef}
                        value={itemDatas}
                        className="p-datatable-sm p-datatable-striped"
                        header={header}
                        loading={this.state.loading}
                        removableSort sortMode="multiple"
                        paginator paginatorTemplate="CurrentPageReport FirstPageLink PrevPageLink PageLinks NextPageLink LastPageLink RowsPerPageDropdown"
                        currentPageReportTemplate="Showing {first} to {last} of {totalRecords}" rows={15} rowsPerPageOptions={[10, 15, 20, 25, 30, 50, 999999]}
                        paginatorLeft={paginatorLeft} paginatorRight={paginatorRight}>
                        {columnComponents}
                    </DataTable>
                </div>
            </div>
        );
    }

    async populateItemData() {
        const response = await fetch('item-data/all');
        const itemDatas = await response.json();

        this.columns = [];
        for (let rawC = 0; rawC < this.rawColumns.length; rawC++) {
            this.columns.push(this.createColumn(itemDatas, this.rawColumns[rawC]));
        }

        this.setState({ itemDatas: itemDatas, loading: false, selectedColumns: this.columns });
    }

    createMultiSelect(arr, propertyName) {
            let stateName = "selected" + propertyName + "s";
            let unique = arr.map(a => a[propertyName]).filter((value, index, self) => self.indexOf(value) === index);
            return (<MultiSelect
                value={this.state[stateName]}
                options={unique}
                onChange={(e) => {
                    this.dt.filter(e.value, propertyName, 'in');
                    var state = {};
                    state[stateName] = e.value;
                    this.setState(state);
                }} placeholder="All"
                className="p-column-filter" />);
    }

    createColumn(itemDatas, rw) {
        if (!rw.multiSelect) {
            return {
                name: rw.field, key: rw.field, column: () => (<Column key={rw.field} field={rw.field} header={rw.field} sortable={rw.sortable} filter={rw.filter} filterMatchMode={rw.filterMatchMode}></Column>)
            };
        } else {
            return {
                name: rw.field, key: rw.field, column: () => (<Column key={rw.field} field={rw.field} header={rw.field} sortable={rw.sortable} filter={rw.filter} filterElement={this.createMultiSelect(itemDatas, rw.field)}></Column>)
            };
        }
    }
}
