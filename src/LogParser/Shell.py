from QueryParser import QueryParser
from Types import SearchSet
import cmd
import sys
from os import path as check_path
import datetime


class Shell(cmd.Cmd):
    """
    Main entry point for LogParser. Starts the interactive query-shell.
    """
    def __init__(self, searchSet: SearchSet):
        super().__init__()       
        self.prompt = "LogParser> "
        self.searchSet = searchSet
        self.results = None

        print("Loading all log-files ....")
        self.queryParser = QueryParser(searchSet)
        self.init_vars()

    def init_vars(self):
        """
        Build a set of variables, based on the look-up dictionary of methods defined in QueryParser
        """
        var_names = self.queryParser.query_methods  # get the keys from Query
        [setattr(self, var, None) for var in var_names]  # Initialize to None

    def catch(func):
        """
        Error-wrapper for query-building commands
        """
        def inner(*args):
            try:
                return func(*args)
            except AttributeError as e:
                print(f"AttributeError - failed to parse command: {e}")
            except ValueError as e:
                print(f"ValueError - failed to parse command: {e}")
            except:
                print("Failed to parse command")
        return inner

    def parse_dates(self, args):
        dates = args.lower().split()

        if len(dates) < 1:
            raise ValueError("No date arguments provided")

        parsed_dates = []
        for i, date in enumerate(dates):
            if date == "today" and i == 0:
                date = datetime.date.today().strftime("%Y-%m-%d") + "-0:0:0.0"
            if date == "today" and i == 1:
                date = datetime.date.today().strftime("%Y-%m-%d") + "-23:59:59.9"
            parsed_dates.append(date)
        return parsed_dates

    def build_query(self):  # init
        """
        Build a dict-object as argument to be passed to QueryParser.
        """
        try:
            # build a dict from all properties with an assigned value
            query_methods = self.queryParser.query_methods
            query_list = {var: self.__getattribute__(var) for var in query_methods if self.__getattribute__(var) is not None}

        except AttributeError as e:
            raise AttributeError(f'Build.build_query(): failed to parse key/value set: {e}')

        return query_list

    def execute_query(self, final_query):
        try:
            self.results = self.queryParser.Parse(final_query)
        except AttributeError as e:
            print(f"Failed to execute query - AttributeError: {e}")
        except ValueError as e:
            print(f"Failed to execute query - ValueError: {e}")
        except:
            print("Failed to execute query.")

    # cli commands - query build
    @catch
    def do_startend(self, args):
        parts = args.split(',')
        if len(parts) < 2: 
            raise ValueError("Must provide minimum 2 words separated by ,")
        part_start = parts[0].split()
        part_end = parts[1].split()
        setattr(self, 'STARTEND', [part_start, part_end])
        setattr(self, 'FIND', None)  # ensure FIND is disabled
        print(f'Adding STARTEND to query: {args}')

    @catch
    def do_find(self, args):
        words = args.split()
        if len(words) < 1: 
            raise ValueError("No search words provided")
        setattr(self, 'FIND', words)
        setattr(self, 'STARTEND', None)   # ensure STARTEND is disabled
        setattr(self, 'SHOWSTATS', None)  # ensure SHOWSTATS is disabled
        print(f'Adding FIND to query: {words}')

    @catch
    def do_between(self, args):
        dates = self.parse_dates(args)
        setattr(self, 'BETWEEN', dates)
        print(f'Adding BETWEEN to query: {dates}')

    @catch
    def do_client(self, args):
        client = args.upper()
        setattr(self, 'CLIENT', client)
        print(f'Adding CLIENT to query: {client}')

    @catch
    def do_sort(self, arg):
        if isinstance(arg, str) and (arg.lower() == 'true'):
            sort = True
        else:
            sort = False
        setattr(self, 'SORT', sort)
        print(f'Adding SORT to query: {sort}')

    @catch
    def do_stats(self, args):
        setattr(self, 'SHOWSTATS', int(args))
        print('Adding SHOWSTATS to query')

    def do_get_clients(self, args):
        clients = self.queryParser.GetClients()
        print(f'Clients: {clients}')

    # cli commands - query management 
    def do_reset(self, args):
        self.init_vars()

    def do_exit(self, args):
        print("Goodbye ....")
        sys.exit()

    def do_run(self, args=None):
        final_query = self.build_query()
        self.execute_query(final_query)
        return self.results

if __name__ == "__main__":
    shell = Shell()
    shell.cmdloop()
