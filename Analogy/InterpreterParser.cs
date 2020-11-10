using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Analogy
{
    public interface IElement
    {
        string Value { get; }
    }
    public class SearchExpression : IElement
    {
        public SearchExpression(string value)
        {
            Value = value;
        }

        public string Value { get; }
    }
    public class SearchOperation : IElement
    {
        private static string EscapeLikeValue(string value)
        {
            StringBuilder sb = new StringBuilder(value.Length);
            foreach (var c in value)
            {
                switch (c)
                {


                    case ']':
                    case '[':
                    case '%':
                    case '*':
                        sb.Append("[").Append(c).Append("]");
                        break;
                    case '\'':
                        sb.Append("''");
                        break;
                    case '@':
                        break;
                    default:
                        sb.Append(c);
                        break;
                }
            }

            return sb.ToString();
        }
        public enum SearchType
        {
            And, Or
        }

        public SearchType Type;
        public IElement Left, Right;

        public string Value
        {
            get
            {
                if (Left == null)
                {
                    return Right.Value;
                }

                if (Right == null)
                {
                    return Left.Value;
                }

                switch (Type)
                {
                    case SearchType.And:
                        return $"Text like '%{EscapeLikeValue(Left.Value)}%' and Text like '%{EscapeLikeValue(Right.Value)}%'";
                    case SearchType.Or:
                        return $"Text like '%{EscapeLikeValue(Left.Value)}%' Or Text like '%{EscapeLikeValue(Right.Value)}%'";
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }

        }
    }

    public enum SearchType
    {
        Expression, And, Or, Lparentheses, Rparentheses
    }
    public class Token
    {
        public string Text { get; set; }
        public SearchType Type { get; set; }

        public Token(string text, SearchType type)
        {
            Text = text;
            Type = type;
        }

        public override string ToString()
        {
            return $"{nameof(Text)}: {Text}";
        }
    }


    public class InterpreterParser
    {
        public static List<Token> Lex(string text)
        {
            var result = new List<Token>();
            for (var i = 0; i < text.Length; i++)
            {
                char c = text[i];
                switch (c)
                {
                    case '+':
                        result.Add(new Token("+", SearchType.And));
                        break;
                    case '&':
                        result.Add(new Token("&", SearchType.And));
                        break;
                    case '|':
                        result.Add(new Token("|", SearchType.Or));
                        break;
                    case '(':
                        result.Add(new Token("(", SearchType.Lparentheses));
                        break;
                    case ')':
                        result.Add(new Token(")", SearchType.Rparentheses));
                        break;
                    default:
                        var sb = new StringBuilder(c.ToString());
                        for (int j = i + 1; j < text.Length; j++)
                        {
                            if (text[j] == '+' || text[j] == '&' || text[j] == '(' || text[j] == ')' || text[j] == '|')
                            {
                                result.Add(new Token(sb.ToString(), SearchType.Expression));
                                break;
                            }
                            else
                            {
                                sb.Append(text[j]);
                                ++i;
                            }
                        }
                        break;
                }
            }

            return result;
        }

        public static IElement Parse(IReadOnlyList<Token> tokens)
        {
            var result = new SearchOperation();
            bool haveLHS = false;
            for (int i = 0; i < tokens.Count; i++)
            {
                var token = tokens[i];
                switch (token.Type)
                {
                    case SearchType.Expression:
                        if (!haveLHS)
                        {
                            result.Left = new SearchExpression(token.Text);
                            haveLHS = true;
                        }
                        else
                        {
                            result.Right = new SearchExpression(token.Text);
                        }
                        break;
                    case SearchType.And:
                        result.Type = SearchOperation.SearchType.And;
                        break;
                    case SearchType.Or:
                        result.Type = SearchOperation.SearchType.Or;
                        break;
                    case SearchType.Lparentheses:
                        int j = i;
                        for (; j < tokens.Count; ++j)

                        {
                            if (tokens[j].Type == SearchType.Rparentheses)
                            {
                                break;
                            }
                        }

                        var subexpression = tokens.Skip(i + 1).Take(j - i - 1).ToList();
                        var element = Parse(subexpression);
                        if (!haveLHS)
                        {
                            result.Left = element;
                            haveLHS = true;
                        }
                        else
                        {
                            result.Right = element;
                        }

                        i = j;
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }

            return result;
        }
    }

}
