import { NzTableSortOrder, NzTableSortFn, NzTableFilterList, NzTableFilterFn } from "ng-zorro-antd/table";

export type NzEnableEditionFn = (data: any) => boolean;

export interface Column {
  title: string;
  field: string;
  width?: string;
  sortable?: boolean;
  filter?: boolean;

  pipe?: string;
  format?: string;
  hidden?: boolean;
  template?: string;

}

export default interface NzColumn extends Column {
  sortOrder: NzTableSortOrder | null;
  sortFn: NzTableSortFn<any> | null;
  enableEditionFn?: NzEnableEditionFn,
  sortDirections: NzTableSortOrder[];
  nzAlign: "center" | "left" | "right" | null;
  className?: string;
  nzEllipsis?: boolean;
}

export interface NzColumnSpan extends NzColumn {
  colSpan: string | number | null;
  rowSpan: string | number | null;
}
