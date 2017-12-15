grammar restructuredtext;

/* https://stackoverflow.com/questions/6178546/antlr-grammar-for-restructuredtext-rule-priorities */

options {
  language=CSharp;
}

tokens {
  ROOT,
  PARAGRAPH,
  INDENTATION,
  LINE,
  WORD,
  BOLD,
  ITALIC,
  INTERPRETED_TEXT,
  INLINE_LITERAL,
  REFERENCE,
}

parse
  :  paragraph+ EOF
  ;

paragraph
  :  line+
  |  Space* LineBreak
  ;

line
  :  indentation text+ LineBreak
  ;

indentation
  :  Space*
  ;

text
  :  styledText
  |  interpretedText
  |  inlineLiteral
  |  reference
  |  Space
  |  Star
  |  EscapeSequence
  |  Any
  ;

styledText
  :  bold
  |  italic
  ;

bold
  :  Star Star boldAtom+ Star Star
  ;  

italic
  :  Star italicAtom+ Star
  ;

boldAtom
  :  ~(Star | LineBreak)
  |  italic
  ;

italicAtom
  :  ~(Star | LineBreak)
  |  bold
  ;

interpretedText
  :  BackTick interpretedTextAtoms BackTick
  ;

interpretedTextAtoms
  :  ~BackTick+
  ;

inlineLiteral
  :  BackTick BackTick inlineLiteralAtoms BackTick BackTick
  ;

inlineLiteralAtoms
  :  inlineLiteralAtom+
  ;

inlineLiteralAtom
  :  ~BackTick
  |  BackTick ~BackTick
  ;

reference
  :  Any+ UnderScore
  ;

UnderScore
  :  '_'
  ;

BackTick
  :  '`'
  ;

Star
  :  '*'
  ;

Space
  :  ' ' 
  |  '\t'
  ;

EscapeSequence
  :  '\\' ('\\' | '*')
  ;

LineBreak
  :  '\r'? '\n'
  |  '\r'
  ;

Any
  :  .
  ;