import React, { Component } from 'react';

export class ItemDataView extends Component {
  static displayName = ItemDataView.name;

  constructor(props) {
    super(props);
    this.state = { itemDatas: [], loading: true };
  }

  componentDidMount() {
    this.populateItemData();
  }

  static renderItemDataTable(itemDatas) {
    return (
      <table className='table table-striped' aria-labelledby="tabelLabel">
        <thead>
          <tr>
            <th>Id</th>
            <th>Name</th>
            <th>Level</th>
            <th>Job</th>
          </tr>
        </thead>
        <tbody>
          {itemDatas.map(itemData =>
              <tr key={itemData.id}>
                  <td>{itemData.id}</td>
                  <td>{itemData.name}</td>
                  <td>{itemData.level}</td>
                  <td>{itemData.job}</td>
            </tr>
          )}
        </tbody>
      </table>
    );
  }

  render() {
    let contents = this.state.loading
      ? <p><em>Loading...</em></p>
      : ItemDataView.renderItemDataTable(this.state.itemDatas);

    return (
      <div>
        <h1 id="tabelLabel">Item Data</h1>
        {contents}
      </div>
    );
  }

  async populateItemData() {
    const response = await fetch('item-data/all');
    const data = await response.json();
    this.setState({ itemDatas: data, loading: false });
  }
}
