import { DataGrid, GridRenderCellParams } from '@mui/x-data-grid';
import React from 'react';

export type RecordsProps = {
}

const Records: React.FC<RecordsProps>  = ({}) => {
	const columns = [
		{
			field: 'title',
			headerName: 'Title',
			flex: 1,
			minWidth: 150,
			renderCell: (params: GridRenderCellParams) => <>{params.value}</>
		},
		{
			field: 'description',
			headerName: 'Description',
			flex: 1,
			minWidth: 150,
			renderCell: (params: GridRenderCellParams) => <>{params.value}</>
		}
	];

	const rawData = [
		{
			title: "Test",
			description: "Test"
		},
		{
			title: "Test",
			description: "Test"
		},
		{
			title: "Test",
			description: "Test"
		},
		{
			title: "Test",
			description: "Test"
		},
		{
			title: "Test",
			description: "Test"
		},
		{
			title: "Test",
			description: "Test"
		},
		{
			title: "Test",
			description: "Test"
		},
		{
			title: "Test",
			description: "Test"
		},
		{
			title: "Test",
			description: "Test"
		},
		{
			title: "Test",
			description: "Test"
		},
		{
			title: "Test",
			description: "Test"
		},
		{
			title: "Test",
			description: "Test"
		},
		{
			title: "Test",
			description: "Test"
		},
		{
			title: "Test",
			description: "Test"
		},
		{
			title: "Test",
			description: "Test"
		},
		{
			title: "Test",
			description: "Test"
		},
		{
			title: "Test",
			description: "Test"
		},
		{
			title: "Test",
			description: "Test"
		},
		{
			title: "Test",
			description: "Test"
		},
		{
			title: "Test",
			description: "Test"
		},
		{
			title: "Test",
			description: "Test"
		},
		{
			title: "Test",
			description: "Test"
		},
		{
			title: "Test",
			description: "Test"
		},
		{
			title: "Test",
			description: "Test"
		}
	];

	let data = rawData.map((item, indx) => ({...item, id: indx}))

	return (
	<div>
		<DataGrid 
        disableRowSelectionOnClick 
        disableColumnSelector 
        rowSelection={false} 
        autoHeight {...data} 
        rows={data} 
        columns={columns} 
        pageSizeOptions={[10, 50, 100]}/>
	</div>);
};

export default Records;
