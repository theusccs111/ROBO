export type MenuIconType = "image" | "icon";

export interface MenuIcon
{
  type: MenuIconType,
  value: string
}

export interface MenuItem
{
  id?: string,
  title: string,
  path: string,
  icon?: MenuIcon,
  items?: MenuItem[]
}

export default interface Menu
{
  id?: string,
  aggregator: string,
  items: MenuItem[]
}
